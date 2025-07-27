''' <summary>
''' Get and Set properties for selected cells of the grid.
''' </summary>
''' <author>Krishna Kishore Mummadi</author>
''' <Creation>09/05/2012</Creation>
''' <remarks>Used to get and set the required properties of the selected cells in the Sales order</remarks>
Public Class clsSelectedCells
    Private _header_id As Double
    Private _line_id As Double
    Private _entity_name As String
    Private _dgv As DataGridView
    Private _row As Integer
    Private _organizationId As Integer
    Private _headerName As String
    Private _shipment_no As Double
    Private _releaseNo As Integer
    Public Property header_id() As Double
        Get
            Return _header_id
        End Get
        Set(ByVal value As Double)
            _header_id = value
        End Set
    End Property
    Public Property line_id() As Double
        Get
            Return _line_id
        End Get
        Set(ByVal value As Double)
            _line_id = value
        End Set
    End Property
    Public Property entity_name() As String
        Get
            Return _entity_name
        End Get
        Set(ByVal value As String)
            _entity_name = value
        End Set
    End Property
    Public Property dgv() As DataGridView
        Get
            Return _dgv
        End Get
        Set(ByVal value As DataGridView)
            _dgv = value
        End Set
    End Property
    Public Property row() As Integer
        Get
            Return _row
        End Get
        Set(ByVal value As Integer)
            _row = value
        End Set
    End Property
    Public Property organizationId() As Integer
        Get
            Return _organizationId
        End Get
        Set(ByVal value As Integer)
            _organizationId = value
        End Set
    End Property
    Public Property headerName() As String
        Get
            Return _headerName
        End Get
        Set(ByVal value As String)
            _headerName = value
        End Set
    End Property
    Public Property shipment_no() As Double
        Get
            Return _shipment_no
        End Get
        Set(ByVal value As Double)
            _shipment_no = value
        End Set
    End Property
    Public Property releaseNo() As Integer
        Get
            Return _releaseNo
        End Get
        Set(ByVal value As Integer)
            _releaseNo = value
        End Set
    End Property
End Class
