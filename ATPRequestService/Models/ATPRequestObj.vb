Public Class ATPRequestObj
    Dim _ORG_CODE As String
    Dim _ITEMS As String
    Dim _ITEM_QTY As Int32
    Dim _LINE_SET As String
    Dim _REQ_DATE_TYPE As String
    Dim _CUST_REQ_DATE As DateTime
    Dim _DEMAND_CLASS As String
    Dim _SHIP_METHOD As String
    Dim _UOM As String
    Dim _CUSTOMER As String
    Dim _CUST_SITE_ID As Int32
    Dim _RCV_ORG As String
    Dim _MODEL As String
    Dim _OFFSET_DAYS As Int32
    Dim _SESSION_ID As Decimal
    Dim _RETURN_STATUS As String
    Dim _RETURN_MSG As String

    Public Property P_ORG_CODE() As String
        Get
            Return _ORG_CODE
        End Get
        Set(value As String)
            _ORG_CODE = value
        End Set
    End Property
    Public Property P_ITEMS As String

        Get

            Return _ITEMS

        End Get
        Set(value As String)
            _ITEMS = value
        End Set
    End Property
    Public Property p_ITEM_QTY As Int32
        Get
            Return _ITEM_QTY
        End Get
        Set(value As Int32)
            _ITEM_QTY = value
        End Set
    End Property
    Public Property P_LINE_SET As String
        Get
            Return _LINE_SET
        End Get
        Set(value As String)
            _LINE_SET = value
        End Set
    End Property
    Public Property P_REQ_DATE_TYPE As String
        Get
            Return _REQ_DATE_TYPE
        End Get
        Set(value As String)
            _REQ_DATE_TYPE = value
        End Set
    End Property
    Public Property P_CUST_REQ_DATE As DateTime
        Get
            Return _CUST_REQ_DATE
        End Get
        Set(value As DateTime)
            _CUST_REQ_DATE = value
        End Set
    End Property
    Public Property P_DEMAND_CLASS As String
        Get
            Return _DEMAND_CLASS
        End Get
        Set(value As String)
            _DEMAND_CLASS = value
        End Set
    End Property
    Public Property P_SHIP_METHOD As String
        Get
            Return _SHIP_METHOD
        End Get
        Set(value As String)
            _SHIP_METHOD = value
        End Set
    End Property
    Public Property P_UOM As String
        Get
            Return _UOM
        End Get
        Set(value As String)
            _UOM = value
        End Set
    End Property
    Public Property P_CUSTOMER As String
        Get
            Return _CUSTOMER
        End Get
        Set(value As String)
            _CUSTOMER = value
        End Set
    End Property
    Public Property P_CUST_SITE_ID As Int32
        Get
            Return _CUST_SITE_ID
        End Get
        Set(value As Int32)
            _CUST_SITE_ID = value
        End Set
    End Property
    Public Property P_RCV_ORG As String
        Get
            Return _RCV_ORG
        End Get
        Set(value As String)
            _RCV_ORG = value
        End Set
    End Property
    Public Property P_MODEL As String
        Get
            Return _MODEL
        End Get
        Set(value As String)
            _MODEL = value
        End Set
    End Property
    Public Property P_OFFSET_DAYS As Int32
        Get
            Return _OFFSET_DAYS
        End Get
        Set(value As Int32)
            _OFFSET_DAYS = value
        End Set
    End Property
    Public Property O_SESSION_ID As Decimal
        Get
            Return _SESSION_ID
        End Get
        Set(value As Decimal)
            _SESSION_ID = value
        End Set
    End Property
    Public Property O_RETURN_STATUS As String
        Get
            Return _RETURN_STATUS
        End Get
        Set(value As String)
            _RETURN_STATUS = value
        End Set
    End Property
    Public Property O_RETURN_MSG As String
        Get
            Return _RETURN_MSG
        End Get
        Set(value As String)
            _RETURN_MSG = value
        End Set
    End Property

End Class

