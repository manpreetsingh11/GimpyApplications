using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

public class Contacts
{
    public int companyid { get; set; }
    public string company { get; set; }
    public string category { get; set; }
    public string email { get; set; }
    public bool status { get; set; }

}