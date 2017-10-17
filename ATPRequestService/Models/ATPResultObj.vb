Public Class ATPResultObj
    Public Sequence_Number As Int32
    Public Session_id As Int32
    Public CALC_AVAILABLE_SHIP_DATE As Date
    Public CALC_GROUP_AVAILABLE_SHIP_DATE As Date
    Public Calc_Offset_Days As Int32
    Public INVENTORY_ITEM_NAME As String
    Public CUSTOM_MATCH As String
    Public SOURCE_ORGANIZATION_CODE As String
    Public QUANTITY_ORDERED As Int32
    Public DEMAND_CLASS As String
    Public SHIP_METHOD As String
    Public Fields As List(Of String)
    Public Values As List(Of String)
    Public IN_REQ_DTE_TYPE As String
    Public IN_CUST_REQ_DATE As Date
    Public REQUESTED_DATE_QUANTITY As Int32
    Public AVAILABLE_QUANTITY As Int32
    Public ERROR_MESSAGE As String



    '	CALC_AVAILABLE_SHIP_DATE  (was CALC_SCHEDULED_SHIP_DATE in Sri's query)
    '	CALC_GROUP_AVAILABLE_SHIP_DATE  (was CALC_GROUP_SHIP_DATE in Sri's query)

    Sub New()
        Fields = New List(Of String)
        Values = New List(Of String)
    End Sub
End Class
