using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

public class Helper
{

    public static SqlConnection CreateConnection()
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["KaintalDBConnectionString"].ConnectionString);
        return con;        
    }

    public static String EncryptData(String data)
    {
        string result = "";

        int i = 2;

        foreach (char item in data)
        {
            result = result + Convert.ToChar(item + 2).ToString() + Convert.ToChar(97 + i).ToString();
            i = i + 2;
            if(i > 25)
            {
                i = 2;
            }
        }
        return result;
    }

    public static void GenerateThumb(String pic_path, String thumb_path, int width, int height)
    {
        Image img = Bitmap.FromFile(pic_path);
        Bitmap bt = new Bitmap(img, width, height);
        bt.Save(thumb_path, ImageFormat.Jpeg);
    }

    public static void ChangeImageSize(Stream strm, String pic_path, int width)
    {
        Image img = new Bitmap(strm);
        int height = (width * img.Height) / img.Width;
        Bitmap bt = new Bitmap(img, width, height);
        bt.Save(pic_path, ImageFormat.Jpeg);        
    }

    public static String GetName(int l)
    {
        string data = "";
        Random rnd = new Random();
        for (int i = 1; i <= l; i++)
        {
            char val = Convert.ToChar(rnd.Next(97, 123));
            data = data + val;
        }
        return data;
    }

    public static String GetRandomNumeric(int l)
    {
        string data = "";
        Random rnd = new Random();
        for (int i = 1; i <= l; i++)
        {
            char val = Convert.ToChar(rnd.Next(48, 58));
            data = data + val;
        }
        return data;
    }

    public static String DecryptData(String data)
    {
        String result = "";
        int i = 0;
        foreach (char item in data)
        {
            if (i % 2 == 0)
            {
                result = result + Convert.ToChar((item-2));
            }
            i++;
        }
        return result;
    }
}