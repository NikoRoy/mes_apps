using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using TDBMESFeeds.Messages;
using System.DirectoryServices.AccountManagement;

namespace BusinessLayer.Controller
{
    public class TDBRecordController : TDBController
    {
        public static HttpClient httpClient = new HttpClient();
        public async Task<string> Create(XElement x)
        {

            //pass xml to mes controller
            var httpContent = new StringContent(x.ToString(), Encoding.UTF8, "application/xml");
            var request = new HttpRequestMessage(HttpMethod.Post, "https://usmer-dmesapp01.getingegroup.local:4401/opexcradapter/messages?wait=true");
            request.Content = httpContent;
            var response = await httpClient.SendAsync(request);
            var b = WasResponseSuccessful(await response.Content.ReadAsStringAsync());
            if (b)
            {
                var u = DeserializeRequest(x);
                b = await UpdateSyncFlag(u);
            }
            return b == true ? await  response.Content.ReadAsStringAsync() : await response.Content.ReadAsStringAsync() + " \n Failure updating the Sync Flag. ";
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string id, XElement x)
        {
            throw new NotImplementedException();
        }
        public TrainingRecordDownloadLite DeserializeRequest(XElement x)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TrainingRecordDownloadLite));
                var reader = x.CreateReader();
                using (reader)
                {
                    return (TrainingRecordDownloadLite)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private bool WasResponseSuccessful(string response)
        {

            XDocument xmlDoc = XDocument.Parse(response);

            var isresponse = from x in xmlDoc.Root.Elements()
                             where x.Name.LocalName == "IsResponse"
                             select x.Value;

            if (isresponse.SingleOrDefault() == "false")
            {
                return false;
            }


            var content = from x in xmlDoc.Root.Elements()
                          where x.Name.LocalName == "Contents"
                          select x.Value;

            var h = XElement.Parse(content.SingleOrDefault());


            var k = h.DescendantsAndSelf().Where(p => p.Name.LocalName == "ErrorDescription").SingleOrDefault();
            if (k != null)
            {
                return false;
            }

            var g = h.DescendantsAndSelf().Where(p => p.Name.LocalName == "CompletionMsg").SingleOrDefault();
            if (g != null)
            {
                return true;
            }

            return false;

        }
        private UserPrincipal GetUserPrincipal(string accountName)
        {
            //Create a user serach prototype in the current domain
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            UserPrincipal user = new UserPrincipal(ctx);

            //Strip any domain part out of the username
            user.SamAccountName = accountName;
            //Perform the search
            PrincipalSearcher searcher = new PrincipalSearcher(user);
            var results = searcher.FindAll();

            //Only return exact matches - there might be more than one John Smith
            if (results.Count() == 1)
            {
                return (UserPrincipal)results.First();
            }

            return null;
        }


        private async Task<bool> UpdateSyncFlag(TrainingRecordDownloadLite r)
        {
            using (SqlConnection connection = new SqlConnection(base.Connection))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "[EmployeeTraining].[dbo].[pLatestMESActions_Update]";


                        var a = GetUserPrincipal(r.EmployeeName);
                        var emp = a.EmailAddress;
                        /*Add Command Parameters here*/
                        cmd.Parameters.Add(new SqlParameter("@EmployeeEmail", emp));
                        cmd.Parameters.Add(new SqlParameter("@DocumentNumber", r.TrainingRequirementName));


                        var res = await cmd.ExecuteNonQueryAsync();

                        if (res <= 0)
                        {
                            return false;
                        }
                        return true;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
