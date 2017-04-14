Imports System.Data.OleDb

Public Class Paramlist
    Inherits List(Of OleDb.OleDbParameter)

    Public Overloads Sub Add(ByVal name As String, ByVal value As Object,
                             Optional ByVal type As Nullable(Of SqlDbType) = Nothing,
                             Optional ByVal direction As ParameterDirection = ParameterDirection.Input)

        Dim tipo As OleDb.OleDbType
        Dim param As OleDbParameter

        If Not type Is Nothing Then
            tipo = type
        Else
            If TypeOf (value) Is Integer Then
                tipo = OleDbType.Integer
            ElseIf TypeOf (value) Is String Then
                tipo = OleDbType.VarChar
            ElseIf TypeOf (value) Is Boolean Then
                tipo = OleDbType.Boolean
            Else
                Throw New Exception("Tipo de dado sql não definido e não identificado")
            End If
        End If

        param = New OleDbParameter()

        'param.DbType = tipo
        param.Direction = direction
        param.OleDbType = tipo
        param.ParameterName = name
        param.Value = value

        Me.Add(param)
    End Sub

    Public Function GetValue(ByVal param As String) As Object
        For Each p As OleDbParameter In Me
            If p.ParameterName = param Then
                Return p.Value
            End If
        Next

        Return Nothing
    End Function

End Class


Public Class DataMagic

#Region "Atributos"

    Dim cn As OleDb.OleDbConnection
    Dim cmd As OleDb.OleDbCommand
    Dim conString As String

    Dim _params As New Paramlist
    Public ReadOnly Property Params As Paramlist
        Get
            Return _params
        End Get
    End Property

#End Region

#Region "Métodos principais"

    Public Sub New(Optional ByVal cnString As String = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Arena;Data Source=localhost")
        conString = cnString
    End Sub

    ''' <summary>
    ''' Abre a conexão e instância a classe command
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StartConection()
        cn = New OleDb.OleDbConnection(conString)
        cn.Open()

        cmd = cn.CreateCommand()

        For Each param As OleDbParameter In Me.Params
            Dim p As New OleDbParameter(param.ParameterName, param.Value)
            p.Direction = param.Direction
            p.OleDbType = param.OleDbType
            cmd.Parameters.Add(p)
        Next

    End Sub

    ''' <summary>
    ''' Recupera os parâmetros de procedure
    ''' após execução
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetParams()
        Params.Clear()
        For Each param As OleDbParameter In cmd.Parameters
            Dim p As New OleDbParameter(param.ParameterName, param.Value)
            p.Direction = param.Direction
            p.OleDbType = param.OleDbType
            Params.Add(p)
        Next

    End Sub

    ''' <summary>
    ''' Fecha possíveis conexões abertas, 
    ''' desaloca command e 
    ''' recupera parâmetros de procedure
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Close()
        GetParams()

        If Not cmd Is Nothing Then
            cmd.Dispose()
        End If

        If cn.State = ConnectionState.Open Then
            cn.Close()
            cn.Dispose()
        End If

    End Sub

#End Region

#Region "Métodos de consulta"

    Public Function GetTable(ByVal procedure As String) As DataTable
        Dim dt As New DataTable
        Dim dap As OleDbDataAdapter

        Me.StartConection()

        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procedure

        dap = New OleDbDataAdapter(cmd)

        dap.Fill(dt)

        Me.Close()

        Return dt

    End Function

    Public Function GetDataset(ByVal procedure As String) As DataSet
        Dim ds As New DataSet
        Dim dap As OleDbDataAdapter

        Me.StartConection()

        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procedure

        dap = New OleDbDataAdapter(cmd)

        dap.Fill(ds)

        Me.Close()

        Return ds

    End Function

    Public Function GetScalar(ByVal procedure As String) As Object
        Dim obj As Object

        Me.StartConection()

        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procedure

        obj = cmd.ExecuteScalar()

        Me.Close()

        Return obj

    End Function

    Public Sub setCommand(ByVal procedure As String)
        Me.StartConection()

        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procedure

        cmd.ExecuteNonQuery()

        Me.Close()

    End Sub

#End Region

End Class

