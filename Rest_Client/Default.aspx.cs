using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Add_Click(object sender, EventArgs e)
    {
        //string address = "http://localhost:35798/RestServiceImpl.svc/Cidade/";
        string address = string.Concat( System.Configuration.ConfigurationManager.AppSettings["URI"], "//Add");
        string result = string.Empty;

        //Carregar XML da nova Cidade do TextBox
        byte[] postData = Encoding.Default.GetBytes(txtCidade.Text);

        HttpWebRequest request = null;
        Uri uri = new Uri(address);
        WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;

        request = (HttpWebRequest)WebRequest.Create(uri);
        request.ContentType = "application/xml";
        request.Method = "POST";

        try
        {
            using (Stream writeStream = request.GetRequestStream())
            {
                writeStream.Write(postData, 0, postData.Length);
            }

            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = resp.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd();

                    }

                }

            }

            Page.Response.Write("Cidade adicionada com sucesso !");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }


        Page.Response.Write("Erro ao adicionar Cidade !");


    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {

    }
}
