using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Azure;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Microsoft.Ajax.Utilities;
using CTNew.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;
using Microsoft.Extensions.Hosting.Internal;
using System.Web;

namespace CTNew.Controllers
{
    
    public class Public : Controller
    {

        public async Task<IActionResult> Index()
        {



            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/current.json?q=53.1%2C-0.13"),
                Headers =
    {
        { "X-RapidAPI-Key", "25b75a2b3fmsh5b2cf05c361361fp1f8999jsn83d0760f45ad" },
        { "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(body);
                // ViewBag.weather = body;

               // var dictionary = JsonConvert.DeserializeObject(body.ToString());

                dynamic data = JsonConvert.DeserializeObject(body, typeof(object));

                var city = JObject.Parse(body).SelectToken("location").ToString();
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(city);

                // var jsonArray = JObject.Parse(body);

                //var name = (string)jsonArray["name"];

                string final = city.ToString().Replace("{", "");
                final = final.Replace("}", "");
                final = final.Replace("\"", "");
                ViewBag.weather = final;
               
            

            }
            ;
            return View("Index");


        }

        [HttpGet]
        public ActionResult FileRead()
        {
            return View("FileRead");
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            try
            {
              
                var filePath = Path.Combine("E:\\VS\\", file.FileName);

                
                if (file.Length > 0)
                {


                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyToAsync(stream);
                    }
                }
                StreamReader sr = new StreamReader(filePath);
                String line;
                line = sr.ReadLine();
                var filecontent = line;
                while (line != null)
                {
                    //write the line to console window

                    //Read the next line
                    line = sr.ReadLine();
                    filecontent = filecontent + "\n" + line;
                }
                //close the file
                sr.Close();

                ViewBag.Message = "File Uploaded Successfully!!";
                ViewBag.FileContent = filecontent;

                return View("FileRead");

            }
            catch(Exception ex) {
                
                ViewBag.Error = "Upload failed";

                return View("FileRead");
            }
            
        }
    } }