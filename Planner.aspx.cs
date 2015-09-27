using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Newtonsoft.Json;
using Microsoft.VisualBasic.FileIO;

namespace goPlan
{
    public partial class Planner : System.Web.UI.Page
    {
        Dictionary<string, dynamic> res1;
        Dictionary<string, dynamic> res2;
        Dictionary<string, dynamic> hotel_keys_values;
        List<string> hotel_ID_list = new List<string>();
        List<string> hotel_names_list = new List<string>();
        string Hotel_ID_map_Name = null;

        List<string> hotel_ID_CSV = new List<string>();
        List<string> hotel_names_CSV = new List<string>();
        Dictionary<string, string> HOtel_dict = new Dictionary<string, string>();
        string place_select = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            
           if(!IsPostBack)
           {
               string csv_file_path = @"~\city_list.csv";
               GetDataTableFromCSVFile(csv_file_path);
               DropDownList1.DataSource = hotel_names_CSV;
               DropDownList1.DataBind();
               DropDownList1.SelectedIndex = 0;

               
           }
                          
        }

        protected void GetDataTableFromCSVFile(string csv_file_path)
        {
            DataTable dt = new DataTable();

            bool IsFirstRowHeader = true;

            string[] columnf = new string[] { "" };

            using (TextFieldParser parser = new TextFieldParser(csv_file_path))
            {

                parser.TrimWhiteSpace = true;

                parser.TextFieldType = FieldType.Delimited;

                parser.SetDelimiters(",");

                if (IsFirstRowHeader)
                {

                    columnf = parser.ReadFields();

                    foreach (string sds in columnf)
                    {

                        DataColumn year = new DataColumn(sds.Trim().ToLower(), Type.GetType("System.String"));

                        dt.Columns.Add(year);

                    }

                }

                while (true)
                {

                    if (IsFirstRowHeader == false)
                    {

                        string[] parts = parser.ReadFields();

                        if (parts == null)
                        {

                            break;

                        }

                        dt.Rows.Add(parts);

                    }

                    IsFirstRowHeader = false;

                }

            }


            foreach (DataRow row in dt.Rows)
            {

                hotel_names_CSV.Add(row["City name"].ToString());
                hotel_ID_CSV.Add(row["City ID"].ToString());

            }

            for (int i = 0; i < hotel_names_CSV.Count; i++)
            {
                if (!HOtel_dict.ContainsKey(hotel_names_CSV[i]))
                {
                    HOtel_dict.Add(hotel_names_CSV[i], hotel_ID_CSV[i]);
                }

            }
            Session["Hotel_lst"] = HOtel_dict;
        }

        protected void find_Click1(object sender, EventArgs e)
        {
            string base_url = "http://developer.goibibo.com/api/voyager/get_hotels_by_cityid/";

            string Auth_url = base_url + "?app_id='your_ID'&app_key='your_key'";
            string C_ID = null;
            HOtel_dict = (Dictionary<string, string>)Session["Hotel_lst"];
            foreach(var city_ID in HOtel_dict)
            {
                if(city_ID.Key==place_select)
                {
                    C_ID = city_ID.Value;
                }
            }

            string url =Auth_url + "&city_id=" +C_ID;
            String result = JsonReaderDocument(url);
            String outputHotelID = null;

            res1 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(result);
            outputHotelID = res1["data"].ToString();
            res2 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(outputHotelID);
            Session["reslt2"] = res2;
            hotel_keys_values = new Dictionary<string, dynamic>();


            foreach (var item in res2)
            {
                hotel_keys_values.Add(item.Key, item.Value);
                hotel_ID_list.Add(item.Key);
            }


            Dictionary<string, dynamic> item1 = new Dictionary<string, dynamic>();
            string str = null, str1 = null, str3 = null;

            foreach (var item in hotel_keys_values)
            {
                str1 = item.Key;
                str = res2[str1].ToString();
                Dictionary<string, dynamic> res3 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str);
                str3 = res3["hotel_geo_node"].ToString();
                Dictionary<string, dynamic> res4 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str3);
                // item1.Add(item.Key,item.Value);
                foreach (var item2 in res4)
                {
                    if (item2.Key == "name")
                    {
                        hotel_names_list.Add(item2.Value);
                    }

                }
            }

            DDL_hotel_names.DataSource = hotel_names_list;
            DDL_hotel_names.DataBind();
            Session["Names_List"] = hotel_names_list;

           
            Session["ID_List"] = hotel_ID_list;
            var data = JsonConvert.SerializeObject(res1["data"].ToString(), Formatting.Indented);
            string s1, s2;
      }
        public static string JsonReaderDocument(string inURL)
        {
            HttpWebRequest myHttpWebRequest = null;     //Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebResponse myHttpWebResponse = null;   //Declare an HTTP-specific implementation of the WebResponse class
            
            string jsonResponse = string.Empty;
            try
            {
                //Create Request
                myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(inURL);
                myHttpWebRequest.Method = "GET";
                myHttpWebRequest.ContentType = "text/json; encoding='utf-8'";

                //Get Response
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();



                using (StreamReader sr = new StreamReader(myHttpWebResponse.GetResponseStream()))
                {


                    jsonResponse = sr.ReadToEnd();


                }

                //  jsonResponse = JsonConvert.SerializeObject(myHttpWebResponse.GetResponseStream(), Formatting.Indented);
            }
            catch (Exception myException)
            {
                throw new Exception("Error Occurred in AuditAdapter.getXMLDocumentFromXMLTemplate()", myException);
            }
            finally
            {
                myHttpWebRequest = null;
                myHttpWebResponse = null;
                myXMLReader = null;
            }
            return jsonResponse;
        }

        protected void HotelInfo_Click(object sender, EventArgs e)
        {
            string str = null, str1 = null, str3 = null, str4 = null, strfacility = null, ss = null;
            string str6 = null, str7 = null,str8=null, strPlace = null;


            hotel_names_list = (List<string>)Session["Names_List"];
            hotel_ID_list = (List<string>)Session["ID_List"];

            //Dictionary<string, string> dict1 = hotel_names_list.ToDictionary(x => x, x => hotel_ID_list[hotel_names_list.IndexOf(x)]);
            Dictionary<string, string> dict1 = new Dictionary<string, string>();
            for (int i = 0; i < hotel_names_list.Count; i++)
            {
                if (!dict1.ContainsKey(hotel_names_list[i]))
                {
                    dict1.Add(hotel_names_list[i], hotel_ID_list[i]);
                }

            }

            foreach (var item in dict1)
            {
                str1 = item.Key;
                if (Hotel_ID_map_Name == str1)
                {
                    ss = item.Value;
                    res2 = (Dictionary<string, dynamic>)Session["reslt2"];
                    str = res2[ss].ToString();
                    Dictionary<string, dynamic> res3 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str);
                    str3 = res3["hotel_data_node"].ToString();
                    Dictionary<string, dynamic> res4 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str3);
                    str4 = res4["facilities"].ToString();
                    Dictionary<string, dynamic> res5 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str4);
                    // item1.Add(item.Key,item.Value);

                    //To extract the facilities
                    foreach (var item2 in res5)
                    {
                        if (item2.Key == "mapped")
                        {
                            var facility = item2.Value;
                            foreach (var item3 in facility)
                            {
                                strfacility = strfacility + item3 + ", ";
                            }
                        }

                    }

                    //To Extract nearby Places
                    str6 = res4["loc"].ToString();
                    Dictionary<string, dynamic> res6 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str6);
                    str7 = res6["nhood"].ToString();
                    List<dynamic> res7 = JsonConvert.DeserializeObject<List<dynamic>>(str7);
                    foreach(var item2 in res7)
                    {
                        str8 = Convert.ToString(item2);
                        Dictionary<string, dynamic> res8 = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str8);
                        foreach(var item3 in res8)
                        {
                             if (item3.Key == "n")
                             {
                                 var places = item3.Value;
                                 strPlace = strPlace + places + ", ";
                             }
                        }
                           if (item2.Key == "n")
                            {
                                
                            }

                        
                    }

                    

                }

            }
            TextArea2.InnerText = strfacility;
            TextArea3.InnerText = strPlace;
        }

        protected void DDL_hotel_names_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hotel_ID_map_Name = DDL_hotel_names.SelectedItem.ToString();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            place_select = DropDownList1.SelectedItem.ToString();
        }




    }
}
