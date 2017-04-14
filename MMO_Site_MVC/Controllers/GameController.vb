Namespace MMO_Site_MVC
    Public Class GameController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /News
        Function News() As ActionResult
            Return View()
        End Function

        ' GET: /Welcome
        Function Welcome() As ActionResult
            Return View()
        End Function

        ' GET: /Download
        Function Download() As ActionResult
            Return View()
        End Function


        ' GET: /Screenshots
        Function Screenshots() As ActionResult
            Return View()
        End Function
    End Class

End Namespace