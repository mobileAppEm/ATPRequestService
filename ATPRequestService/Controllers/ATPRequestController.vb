Imports System.Net
Imports System.Web.Http
Imports Oracle.ManagedDataAccess

Namespace Controllers
    Public Class ATPRequestController
        Inherits ApiController
        Dim _connection As Oracle.ManagedDataAccess.Client.OracleConnection
        <Route("ATPRequestSessionID")>
        Public Function ATPRequestSessionID(reqobj As ATPRequestObj) As Int32
            GetDBConnection()

            With ExecuteATP(reqobj)
                If .O_RETURN_MSG.Length > 0 Then
                    Return 0
                Else
                    Return .O_SESSION_ID
                End If
            End With

        End Function
        <Route("ATPRequest")>
        Public Function ATPRequest(<FromBody> reqobj As ATPRequestObj) As List(Of ATPResultObj)
            GetDBConnection()

            Return RetrieveResults(ExecuteATP(reqobj))
        End Function
        Private Function GetOracleconnectionstring() As String
            Return ConfigurationManager.ConnectionStrings.Item("OracleConnection").ToString
            'Return "user id=VLVSATP_IU;password=G3n#r1c4K1;data source=" +
            '"(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)" +
            '"(HOST=usmtnpmdinfdb83.dev.emrsn.org)(PORT=35601))(CONNECT_DATA=" +
            '"(SERVICE_NAME=BetsyK1)))"
        End Function
        Private Function connecttoOracledb() As Oracle.ManagedDataAccess.Client.OracleConnection
            Return (New Client.OracleConnection(GetOracleconnectionstring))
        End Function
        <Route("TestDBConnection")>
        Public Function testdbconnection() As Boolean
            Dim l_connection As New Oracle.ManagedDataAccess.Client.OracleConnection
            l_connection.ConnectionString = GetOracleconnectionstring()
            l_connection.Open()
            Return True
        End Function
        Private Sub GetDBConnection()
            If _connection Is Nothing Then
                _connection = connecttoOracledb()
                _connection.Open()
            ElseIf _connection.State <> ConnectionState.Open Then
                _connection = connecttoOracledb()
                _connection.Open()
            End If

        End Sub
        Private Function RetrieveResults(_OracleReq As ATPRequestObj) As List(Of ATPResultObj)
            Dim l_list As New List(Of ATPResultObj)

            If _OracleReq.O_SESSION_ID = 0 Then
                Dim l_return As New ATPResultObj
                l_return.ERROR_MESSAGE = _OracleReq.O_RETURN_MSG
                l_return.Session_id = _OracleReq.O_SESSION_ID
                l_list.Add(l_return)
                Return l_list
            End If

            With _connection.CreateCommand()
                .CommandText = "Select MAST.session_id, MAST.COMPILE_DESIGNATOR Plan_Name, MAST.SOURCE_ORGANIZATION_CODE, " & _
                    " MAST.INVENTORY_ITEM_NAME,(SELECT REPLACE (x.em_string, '.', '-') FROM apps.xxbom_cust_matching_vlvs x " & _
                    " WHERE preconfigured_item_id = MAST.inventory_item_id AND x.organization_id = MAST.source_organization_id AND NVL (start_date, SYSDATE) <= SYSDATE AND NVL (end_date, SYSDATE) >= SYSDATE) Custom_Match, MAST.QUANTITY_ORDERED, MAST.UOM_CODE, XMRP.IN_REQ_DTE_TYPE , MAST.REQUESTED_DATE_QUANTITY," & _
                    " MAST.AVAILABLE_QUANTITY, MAST.GROUP_SHIP_DATE, MAST.GROUP_ARRIVAL_DATE, MAST.scheduled_arrival_date," & _
                    " MAST.SHIP_METHOD, MAST.demand_class, MAST.ERROR_MESSAGE, MAST.sequence_number, XMRP.IN_CUST_REQ_DATE," & _
                    " XMRP.CALC_OFFSET_DAYS," & _
                    " APPS.XXMRP_FF2_ORC_ATP_VLVS_PKG.xxom_add_sub_lead_time ('ADD',MAST.SCHEDULED_SHIP_DATE,XMRP.CALC_OFFSET_DAYS,XMRP.SHIP_CALENDAR_CODE) Calc_Available_SHIP_DATE," & _
                    " APPS.XXMRP_FF2_ORC_ATP_VLVS_PKG.xxom_add_sub_lead_time ('ADD',MAST.GROUP_SHIP_DATE,XMRP.CALC_OFFSET_DAYS,XMRP.SHIP_CALENDAR_CODE) Calc_GROUP_Available_SHIP_DATE," & _
                    " APPS.XXMRP_FF2_ORC_ATP_VLVS_PKG.xxom_add_sub_lead_time ('ADD',MAST.scheduled_arrival_date,XMRP.CALC_OFFSET_DAYS,XMRP.SHIP_CALENDAR_CODE) Calc_scheduled_arrival_date," & _
                    " APPS.XXMRP_FF2_ORC_ATP_VLVS_PKG.xxom_add_sub_lead_time ('ADD',MAST.GROUP_ARRIVAL_DATE,XMRP.CALC_OFFSET_DAYS,XMRP.SHIP_CALENDAR_CODE) Calc_GROUP_ARRIVAL_DATE " & _
                    " From APPS.MRP_ATP_SCHEDULE_TEMP_V MAST, XXMRP.XXMRP_ATP_INQ_DTLS_EPM XMRP Where status_flag = 2 And mast.session_id =  & Math.Abs(_OracleReq.O_SESSION_ID) &  And XMRP.session_id = mast.session_id ORDER BY SEQUENCE_NUMBER"
                Dim dr As Client.OracleDataReader = .ExecuteReader
                Dim resultString As String
                resultString = ""
                dr.Read()
                If dr.HasRows Then


                    Dim l_t As New ATPResultObj
                    Do



                        l_t = New ATPResultObj
                        l_t.Session_id = Math.Abs(_OracleReq.O_SESSION_ID)
                        l_t.Sequence_Number = dr.Item("Sequence_Number").ToString
                        l_t.INVENTORY_ITEM_NAME = dr.Item("INVENTORY_ITEM_NAME").ToString
                        l_t.AVAILABLE_QUANTITY = IIf(CType(dr.Item("AVAILABLE_QUANTITY").ToString, Int64) > 10000000, 1000000, CType(dr.Item("AVAILABLE_QUANTITY").ToString, Int64))
                        If dr.Item("CALC_AVAILABLE_SHIP_DATE").ToString <> "" Then
                            l_t.CALC_AVAILABLE_SHIP_DATE = CType(dr.Item("CALC_AVAILABLE_SHIP_DATE").ToString, Date)
                        End If


                        If dr.Item("CALC_GROUP_AVAILABLE_SHIP_DATE").ToString() <> "" Then
                            l_t.CALC_GROUP_AVAILABLE_SHIP_DATE = CType(dr.Item("CALC_GROUP_AVAILABLE_SHIP_DATE").ToString, Date)
                        End If
                        l_t.Calc_Offset_Days = CType(dr.Item("Calc_Offset_Days").ToString, Int32)
                        l_t.CUSTOM_MATCH = dr.Item("CUSTOM_MATCH").ToString
                        l_t.DEMAND_CLASS = dr.Item("DEMAND_CLASS").ToString
                        l_t.IN_CUST_REQ_DATE = CType(dr.Item("IN_CUST_REQ_DATE").ToString, Date)
                        l_t.IN_REQ_DTE_TYPE = dr.Item("IN_REQ_DTE_TYPE").ToString
                        l_t.QUANTITY_ORDERED = CType(dr.Item("QUANTITY_ORDERED").ToString, Int32)
                        l_t.REQUESTED_DATE_QUANTITY = If(CType(dr.Item("REQUESTED_DATE_QUANTITY").ToString, Int64) > 100000000, 1000000, CType(dr.Item("REQUESTED_DATE_QUANTITY").ToString, Int64))
                        l_t.SHIP_METHOD = dr.Item("SHIP_METHOD").ToString
                        l_t.SOURCE_ORGANIZATION_CODE = dr.Item("SOURCE_ORGANIZATION_CODE").ToString
                        l_t.ERROR_MESSAGE = dr.Item("ERROR_MESSAGE").ToString

                        For a = 0 To dr.FieldCount - 1
                            l_t.Fields.Add(dr.GetName(a).ToString)
                        Next
                        For a = 0 To dr.FieldCount - 1
                            l_t.Values.Add(dr.GetValue(a).ToString)
                        Next
                        l_list.Add(l_t)
                    Loop While dr.Read




                    Return l_list
                ElseIf _OracleReq.O_RETURN_MSG <> "" Then
                    Dim l_return As New ATPResultObj
                    l_return.ERROR_MESSAGE = _OracleReq.O_RETURN_MSG
                    l_return.INVENTORY_ITEM_NAME = _OracleReq.P_ITEMS
                    l_list.Add(l_return)
                    Return l_list

                ElseIf _OracleReq.O_RETURN_STATUS <> "" Then


                Else
                    Dim l_return As New ATPResultObj
                    l_return.ERROR_MESSAGE = "No Results for _id " & Math.Abs(_OracleReq.O_SESSION_ID) & " " & resultString
                    l_list.Add(l_return)
                    Return l_list
                End If


            End With
            Return l_list
        End Function
        Private Function ExecuteATP(Optional reqobj As ATPRequestObj = Nothing) As ATPRequestObj
            'If reqobj Is Nothing Then
            '    reqobj = New ATPRequestObj
            '    reqobj.P_ORG_CODE = "MTN"
            '    reqobj.P_ITEMS = "ECX1-A151-B1-C424-E1-F26-G5-H1-J1-9A6-9D13-9E35;EX1-A2-B1-C2-D374;667X1-A1-B25-C7-D10-E4-F7-H1-J1-K1-L1-9A31-9B13-9C1-9D1;DVC6200X1-A8-B2-C1-D14-E83-F9-G4-H5-J12-9A3-9C1-9D4-9M1-9P1-9Q5;NCMTGX1-A654-B1-C9-9A1-9B1-9D1-9F1"
            '    reqobj.p_ITEM_QTY = 1
            '    reqobj.P_LINE_SET = "ARRIVAL"
            '    reqobj.P_REQ_DATE_TYPE = "ARRIVAL"
            '    reqobj.P_CUST_REQ_DATE = "8/20/2017"
            '    reqobj.P_CUSTOMER = ""
            '    reqobj.P_DEMAND_CLASS = ""
            '    reqobj.P_CUST_SITE_ID = 0
            '    reqobj.P_MODEL = ""
            '    reqobj.P_OFFSET_DAYS = 10
            '    reqobj.P_RCV_ORG = ""
            '    reqobj.P_SHIP_METHOD = ""
            '    reqobj.P_UOM = ""
            'End If
            Dim l_command As Oracle.ManagedDataAccess.Client.OracleCommand
            l_command = New Client.OracleCommand()
            l_command.Connection = _connection
            l_command.CommandType = CommandType.StoredProcedure
            l_command.CommandText = "APPS.XXMRP_FF2_ORC_ATP_EPM_PKG.Call_ATP"
            l_command.BindByName = True
            With l_command.Parameters.Add("P_CUSTOMER", Client.OracleDbType.Char)
                .Size = 10
                .Value = reqobj.P_CUSTOMER
            End With
            With l_command.Parameters.Add("P_CUST_REQ_DATE", Client.OracleDbType.Date)
                .Value = reqobj.P_CUST_REQ_DATE
            End With
            With l_command.Parameters.Add("P_CUST_SITE_ID", Client.OracleDbType.Int32)
                .Value = reqobj.P_CUST_SITE_ID
            End With
            With l_command.Parameters.Add("P_DEMAND_CLASS", Client.OracleDbType.Char)
                .Size = 10
                .Value = reqobj.P_DEMAND_CLASS
            End With
            With l_command.Parameters.Add("P_ITEMS", Client.OracleDbType.Char)
                .Size = 8000
                .Value = reqobj.P_ITEMS
            End With
            With l_command.Parameters.Add("p_ITEM_QTY", Client.OracleDbType.Int32)
                .Value = reqobj.p_ITEM_QTY
            End With
            With l_command.Parameters.Add("P_LINE_SET", Client.OracleDbType.Char)
                .Size = 10
                .Value = reqobj.P_LINE_SET
            End With
            With l_command.Parameters.Add("P_MODEL", Client.OracleDbType.Char)
                .Size = 10
                .Value = reqobj.P_MODEL
            End With
            With l_command.Parameters.Add("P_OFFSET_DAYS", Client.OracleDbType.Int32)
                .Value = reqobj.P_OFFSET_DAYS
            End With
            With l_command.Parameters.Add("P_ORG_CODE", Client.OracleDbType.Char)
                .Size = 10
                .Value = reqobj.P_ORG_CODE
            End With
            With l_command.Parameters.Add("P_RCV_ORG", Client.OracleDbType.Char)
                .Size = 10
                .Value = reqobj.P_RCV_ORG
            End With
            With l_command.Parameters.Add("P_REQ_DATE_TYPE", Client.OracleDbType.Char)
                .Size = 10
                .Value = reqobj.P_REQ_DATE_TYPE
            End With
            With l_command.Parameters.Add("P_SHIP_METHOD", Client.OracleDbType.Char)
                .Size = 10
                .Value = reqobj.P_SHIP_METHOD
            End With
            With l_command.Parameters.Add("P_UOM", Client.OracleDbType.Char)
                .Size = 10
                .Value = reqobj.P_UOM
            End With

            With l_command.Parameters.Add("O_SESSION_ID", Client.OracleDbType.Int32)
                .Direction = ParameterDirection.Output
            End With
            With l_command.Parameters.Add("O_RETURN_STATUS", Client.OracleDbType.Char)
                .Size = 100
                .Direction = ParameterDirection.Output
            End With
            With l_command.Parameters.Add("O_RETURN_MSG", Client.OracleDbType.Char)
                .Size = 4000
                .Direction = ParameterDirection.Output
            End With
            Dim da = New Client.OracleDataAdapter(l_command)
            l_command.ExecuteNonQuery()
            reqobj.O_SESSION_ID = Convert.ToDecimal(l_command.Parameters.Item("O_SESSION_ID").Value.ToString)
            reqobj.O_RETURN_MSG = l_command.Parameters.Item("O_RETURN_MSG").Value.ToString
            reqobj.O_RETURN_STATUS = l_command.Parameters.Item("O_RETURN_STATUS").Value.ToString
            Return reqobj
            da.Dispose()
            l_command.Dispose()
            _connection.Close()
            'New Client.OracleParameter("P_ORG_CODE", Client.OracleDbType.Char(8000)))
        End Function


    End Class
End Namespace