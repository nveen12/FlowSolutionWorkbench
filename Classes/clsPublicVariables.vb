Public Class clsPublicVariables
    Public ReadOnly Property nameOfTool()
        Get
            Return "Autobahn"
        End Get
    End Property
    Public ReadOnly Property connectionStringStar()
        Get
            'Return "Data Source=ST1PROD;User ID=fls_query;Password=st5v8a4v;Unicode=True"
            Return "Data Source=ST1PROD;User ID=fls_query;Password=st5v8a4v;Unicode=True"
        End Get
    End Property

    Public ReadOnly Property connectionStringEDC()
        Get
            Return "Data Source=PROD;User ID=fls_query;Password=bQi_l2_Sr0xMMin;Unicode=True"
        End Get
    End Property
End Class
