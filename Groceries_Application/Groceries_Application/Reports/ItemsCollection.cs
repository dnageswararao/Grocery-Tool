﻿using System.Drawing;
using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Groceries_Application.Reports
{
    [DataObject]
    public class ItemsCollection
    {

        static readonly SqlConnection Cnn1 = DbUtility.GetConnection();
        //const string ConnetionString =
        //        "Server = TECHNO5/SQLEXPRESS;Data Source=TECHNO5;Initial Catalog=PDFReaderDb;Persist Security Info=True;User ID=sa;Password=Design_20";


        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IList<ItemsList> GetVendorAllItems(string vendortextbox)
        {
            SqlConnection cnn;
            using (cnn = new SqlConnection(Cnn1.ConnectionString))
            {
                var query = vendortextbox.Contains("DIP")
                    ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 where Amount <>0" + "order by InvoiceDate asc"
                    : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable where  Amount <>0" + "order by InvoiceDate asc";
                cnn.Open();

                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);


                ItemsDataCollection = new List<ItemsList>();

                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var desc = ds.Tables[0].Rows;
                    var query1 = vendortextbox.Contains("DIP")
                        ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 where Description = '" +
                          desc[i].ItemArray[3] + "'and Amount <>0" +
                          "order by InvoiceDate asc"
                        : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable where Description = '" +
                          desc[i].ItemArray[3] + "'and Amount <>0" +
                          "order by InvoiceDate asc";
                    var cmd1 = new SqlCommand(query1, cnn);
                    var sda1 = new SqlDataAdapter(cmd1);
                    var ds1 = new DataSet();
                    PercentageDataCollection = new List<ItemsList>();
                    sda1.Fill(ds1);

                    var pricecount = (from DataRow dr in ds1.Tables[0].Rows select dr.ItemArray).ToList();
                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i1 = 0; i1 < pricecount.Count - 1; i1++)
                        {
                            var percentage = Convert.ToDouble(ds1.Tables[0].Rows[i1 + 1].ItemArray[6]) -
                                             Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6]);
                            percentage = (percentage / Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6])) * 100;
                            var perce = new ItemsList
                            {
                                PercentageValue =
                                    percentage != 0 ? Convert.ToDouble(percentage.ToString("##.##")) : percentage
                            };
                            PercentageDataCollection.Add(perce);
                        }
                    }

                    var perccollection = PercentageDataCollection.Select(it => it.PercentageValue).ToList();

                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i2 = 0; i2 < pricecount.Count; i2++)
                        {
                            var d = new ItemsList
                            {
                                InvoiceNo = ds1.Tables[0].Rows[i2].ItemArray[0].ToString(),
                                InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[i2].ItemArray[1]),
                                Item = ds1.Tables[0].Rows[i2].ItemArray[2].ToString(),
                                Description = ds1.Tables[0].Rows[i2].ItemArray[3].ToString(),
                                Unit = ds1.Tables[0].Rows[i2].ItemArray[4].ToString(),
                                Shipped = ds1.Tables[0].Rows[i2].ItemArray[5].ToString(),
                                Price = Convert.ToDouble(ds1.Tables[0].Rows[i2].ItemArray[6].ToString()),
                                PercentageValue = i2 != 0 ? perccollection[i2 - 1] : 0
                            };
                            {
                                ItemsDataCollection.Add(d);
                            }
                        }
                    }

                    else if (ds1.Tables[0].Rows.Count <= 1 && ds1.Tables[0].Rows.Count != 0)
                    {
                        var d = new ItemsList
                        {
                            InvoiceNo = ds1.Tables[0].Rows[0].ItemArray[0].ToString(),
                            InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[0].ItemArray[1]),
                            Item = ds1.Tables[0].Rows[0].ItemArray[2].ToString(),
                            Description = ds1.Tables[0].Rows[0].ItemArray[3].ToString(),
                            Unit = ds1.Tables[0].Rows[0].ItemArray[4].ToString(),
                            Shipped = ds1.Tables[0].Rows[0].ItemArray[5].ToString(),
                            Price = Convert.ToDouble(ds1.Tables[0].Rows[0].ItemArray[6].ToString()),
                            PercentageValue = 0
                        };
                        {
                            ItemsDataCollection.Add(d);
                        }
                    }
                }
                return ItemsDataCollection;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IList<ItemsList> GetOnlyVendorBasedResults(string vendortextbox)
        {
            SqlConnection cnn;
            using (cnn = new SqlConnection(Cnn1.ConnectionString))
            {
                var query = vendortextbox.Contains("DIP")
                    ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 order by InvoiceDate asc"
                    : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable order by InvoiceDate asc";
                cnn.Open();

                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);


                ItemsDataCollection = new List<ItemsList>();

                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var desc = ds.Tables[0].Rows;
                    var query1 = vendortextbox.Contains("DIP")
                        ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 where Description = '" +
                          desc[i].ItemArray[3] + "'" + "order by InvoiceDate asc"
                        : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable where Description = '" +
                          desc[i].ItemArray[3] + "'" + "order by InvoiceDate asc";
                    var cmd1 = new SqlCommand(query1, cnn);
                    var sda1 = new SqlDataAdapter(cmd1);
                    var ds1 = new DataSet();
                    PercentageDataCollection = new List<ItemsList>();
                    sda1.Fill(ds1);

                    var pricecount = (from DataRow dr in ds1.Tables[0].Rows select dr.ItemArray).ToList();
                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i1 = 0; i1 < pricecount.Count - 1; i1++)
                        {
                            var percentage = Convert.ToDouble(ds1.Tables[0].Rows[i1 + 1].ItemArray[6]) -
                                             Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6]);
                            percentage = (percentage / Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6])) * 100;
                            var perce = new ItemsList
                            {
                                PercentageValue =
                                    percentage != 0 ? Convert.ToDouble(percentage.ToString("##.##")) : percentage
                            };
                            PercentageDataCollection.Add(perce);
                        }
                    }

                    var perccollection = PercentageDataCollection.Select(it => it.PercentageValue).ToList();

                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i2 = 0; i2 < pricecount.Count; i2++)
                        {
                            var d = new ItemsList
                            {
                                InvoiceNo = ds1.Tables[0].Rows[i2].ItemArray[0].ToString(),
                                InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[i2].ItemArray[1]),
                                Item = ds1.Tables[0].Rows[i2].ItemArray[2].ToString(),
                                Description = ds1.Tables[0].Rows[i2].ItemArray[3].ToString(),
                                Unit = ds1.Tables[0].Rows[i2].ItemArray[4].ToString(),
                                Shipped = ds1.Tables[0].Rows[i2].ItemArray[5].ToString(),
                                Price = Convert.ToDouble(ds1.Tables[0].Rows[i2].ItemArray[6].ToString()),
                                PercentageValue = i2 != 0 ? perccollection[i2 - 1] : 0
                            };
                            {
                                ItemsDataCollection.Add(d);
                            }
                        }
                    }

                    else if (ds1.Tables[0].Rows.Count <= 1 && ds1.Tables[0].Rows.Count != 0)
                    {
                        var d = new ItemsList
                        {
                            InvoiceNo = ds1.Tables[0].Rows[0].ItemArray[0].ToString(),
                            InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[0].ItemArray[1]),
                            Item = ds1.Tables[0].Rows[0].ItemArray[2].ToString(),
                            Description = ds1.Tables[0].Rows[0].ItemArray[3].ToString(),
                            Unit = ds1.Tables[0].Rows[0].ItemArray[4].ToString(),
                            Shipped = ds1.Tables[0].Rows[0].ItemArray[5].ToString(),
                            Price = Convert.ToDouble(ds1.Tables[0].Rows[0].ItemArray[6].ToString()),
                            PercentageValue = 0
                        };
                        {
                            ItemsDataCollection.Add(d);
                        }
                    }


                }

                return ItemsDataCollection;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IList<ItemsList> GetVendorBasedResults(string fromdate, string todate, string vendortextbox)
        {
            SqlConnection cnn;
            using (cnn = new SqlConnection(Cnn1.ConnectionString))
            {
                var query = vendortextbox.Contains("DIP")
                    ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 where InvoiceDate >= '" + fromdate + "' and InvoiceDate <= '" + todate + "'" + "and Amount <>0" + "order by InvoiceDate asc"
                    : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable where InvoiceDate >= '" + fromdate + "' and InvoiceDate <= '" + todate + "'" + "and Amount <>0" + "order by InvoiceDate asc";
                cnn.Open();

                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);


                ItemsDataCollection = new List<ItemsList>();

                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var desc = ds.Tables[0].Rows;
                    var query1 = vendortextbox.Contains("DIP")
                        ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 where Description = '" +
                          desc[i].ItemArray[3] + "'" + "and InvoiceDate >= '" + fromdate + "' and InvoiceDate <= '" +
                          todate + "'" + "and Amount <>0" +
                          "order by InvoiceDate asc"
                        : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable where Description = '" +
                          desc[i].ItemArray[3] + "'" + "and InvoiceDate >= '" + fromdate + "' and InvoiceDate <= '" +
                          todate + "'" + "and Amount <>0" +
                          "order by InvoiceDate asc";
                    var cmd1 = new SqlCommand(query1, cnn);
                    var sda1 = new SqlDataAdapter(cmd1);
                    var ds1 = new DataSet();
                    PercentageDataCollection = new List<ItemsList>();
                    sda1.Fill(ds1);

                    var pricecount = (from DataRow dr in ds1.Tables[0].Rows select dr.ItemArray).ToList();
                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i1 = 0; i1 < pricecount.Count - 1; i1++)
                        {
                            var percentage = Convert.ToDouble(ds1.Tables[0].Rows[i1 + 1].ItemArray[6]) -
                                             Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6]);
                            percentage = (percentage / Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6])) * 100;
                            var perce = new ItemsList
                            {
                                PercentageValue =
                                    percentage != 0 ? Convert.ToDouble(percentage.ToString("##.##")) : percentage
                            };
                            PercentageDataCollection.Add(perce);
                        }
                    }

                    var perccollection = PercentageDataCollection.Select(it => it.PercentageValue).ToList();

                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i2 = 0; i2 < pricecount.Count; i2++)
                        {
                            var d = new ItemsList
                            {
                                InvoiceNo = ds1.Tables[0].Rows[i2].ItemArray[0].ToString(),
                                InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[i2].ItemArray[1]),
                                Item = ds1.Tables[0].Rows[i2].ItemArray[2].ToString(),
                                Description = ds1.Tables[0].Rows[i2].ItemArray[3].ToString(),
                                Unit = ds1.Tables[0].Rows[i2].ItemArray[4].ToString(),
                                Shipped = ds1.Tables[0].Rows[i2].ItemArray[5].ToString(),
                                Price = Convert.ToDouble(ds1.Tables[0].Rows[i2].ItemArray[6].ToString()),
                                PercentageValue = i2 != 0 ? perccollection[i2 - 1] : 0
                            };
                            {
                                ItemsDataCollection.Add(d);
                            }
                        }
                    }

                    else if (ds1.Tables[0].Rows.Count <= 1 && ds1.Tables[0].Rows.Count != 0)
                    {
                        var d = new ItemsList
                        {
                            InvoiceNo = ds1.Tables[0].Rows[0].ItemArray[0].ToString(),
                            InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[0].ItemArray[1]),
                            Item = ds1.Tables[0].Rows[0].ItemArray[2].ToString(),
                            Description = ds1.Tables[0].Rows[0].ItemArray[3].ToString(),
                            Unit = ds1.Tables[0].Rows[0].ItemArray[4].ToString(),
                            Shipped = ds1.Tables[0].Rows[0].ItemArray[5].ToString(),
                            Price = Convert.ToDouble(ds1.Tables[0].Rows[0].ItemArray[6].ToString()),
                            PercentageValue = 0
                        };
                        {
                            ItemsDataCollection.Add(d);
                        }
                    }
                }
                return ItemsDataCollection;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IList<ItemsList> GetItemVendorBasedResults(string fromdate, string todate, string vendortextbox, string desctext)
        {
            SqlConnection cnn;
            using (cnn = new SqlConnection(Cnn1.ConnectionString))
            {
                var query = vendortextbox.Contains("DIP")
                    ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 where Description = '" +
                      desctext + "'" + "and InvoiceDate >= '" + fromdate + "' and InvoiceDate <= '" + todate + "'" +
                      "and Amount <>0" +
                      "order by InvoiceDate asc"
                    : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable where Description = '" +
                      desctext + "'" + "and InvoiceDate >= '" + fromdate + "' and InvoiceDate <= '" + todate + "'" +
                      "and Amount <>0" +
                      "order by InvoiceDate asc";
                cnn.Open();

                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);

                PercentageDataCollection = new List<ItemsList>();
                ItemsDataCollection = new List<ItemsList>();
                var pricecount = (from DataRow dr in ds.Tables[0].Rows select dr.ItemArray).ToList();

                for (var i = 0; i < pricecount.Count - 1; i++)
                {
                    var percentage = Convert.ToDouble(ds.Tables[0].Rows[i + 1].ItemArray[6]) -
                                        Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[6]);
                    percentage = (percentage / Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[6])) * 100;
                    var perce = new ItemsList()
                    {
                        PercentageValue = percentage != 0 ? Convert.ToDouble(percentage.ToString("##.##")) : percentage
                    };
                    PercentageDataCollection.Add(perce);
                }

                var perccollection = PercentageDataCollection.Select(it => it.PercentageValue).ToList();

                for (var i = 0; i < pricecount.Count; i++)
                {
                    var d = new ItemsList
                    {
                        InvoiceNo = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                        InvoiceDate = Convert.ToDateTime(ds.Tables[0].Rows[i].ItemArray[1]),
                        Item = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                        Description = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                        Unit = ds.Tables[0].Rows[i].ItemArray[4].ToString(),
                        Shipped = ds.Tables[0].Rows[i].ItemArray[5].ToString(),
                        Price = Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[6].ToString()),
                        PercentageValue = i != 0 ? perccollection[i - 1] : 0
                    };
                    {
                        ItemsDataCollection.Add(d);
                    }
                }

                return ItemsDataCollection;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static double GetPercentage(string vendor, string desctext, string fromdate, string todate)
        {
            SqlConnection cnn;
            using (cnn = new SqlConnection(Cnn1.ConnectionString))
            {
                var query = vendor.Contains("DIP")
                     ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 where Description = '" + desctext + "'" + "and InvoiceDate >= '" + fromdate + "' and InvoiceDate <= '" + todate + "'" + "and Amount <>0" + "order by InvoiceDate asc"
                     : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable where Description = '" + desctext + "'" + "and InvoiceDate >= '" + fromdate + "' and InvoiceDate <= '" + todate + "'" + "and Amount <>0" + "order by InvoiceDate asc";
                cnn.Open();

                double percentage = 0;
                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);
                ItemsDataCollection = new List<ItemsList>();

                var products = (from DataRow dr in ds.Tables[0].Rows select dr.ItemArray).ToList();
                if (products.Count <= 1) return percentage;
                for (var i = products.Count; i <= products.Count; i++)
                {
                    percentage = Convert.ToDouble(ds.Tables[0].Rows[i - 1].ItemArray[6]) -
                                 Convert.ToDouble(ds.Tables[0].Rows[i - i].ItemArray[6]);
                    percentage = (percentage / Convert.ToDouble(ds.Tables[0].Rows[i - i].ItemArray[6])) * 100;
                }

                return percentage != 0 ? Convert.ToDouble(percentage.ToString("##.##")) : percentage;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static double GetPercentageTop10(string vendor, string desctext)
        {
            SqlConnection cnn;
            using (cnn = new SqlConnection(Cnn1.ConnectionString))
            {
                var query = vendor.Contains("DIP")
                     ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 where Description = '" + desctext + "'" + "and Amount <>0" + "order by InvoiceDate asc"
                     : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable where Description = '" + desctext + "'" + "and Amount <>0" + "order by InvoiceDate asc";
                cnn.Open();

                double percentage = 0;
                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);
                ItemsDataCollection = new List<ItemsList>();

                var products = (from DataRow dr in ds.Tables[0].Rows select dr.ItemArray).ToList();
                if (products.Count <= 1) return percentage;
                for (var i = products.Count; i <= products.Count; i++)
                {
                    //var per = Convert.ToDouble(ds.Tables[0].Rows[i - 1].ItemArray[6]);
                    //var shipped = Convert.ToDouble(ds.Tables[0].Rows[i - 1].ItemArray[5]);
                    //var value = Convert.ToDouble(per/shipped);
                    percentage = Convert.ToDouble(ds.Tables[0].Rows[i - 1].ItemArray[6]) -
                                 Convert.ToDouble(ds.Tables[0].Rows[i - i].ItemArray[6]);
                    percentage = (percentage / Convert.ToDouble(ds.Tables[0].Rows[i - i].ItemArray[6])) * 100;
                }

                return percentage != 0 ? Convert.ToDouble(percentage.ToString("##.##")) : percentage;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IList<ItemsList> GetTop10Rate(string vendor)
        {
            SqlConnection cnn;
            using (cnn = new SqlConnection(Cnn1.ConnectionString))
            {
                var query = vendor.Contains("DIP")
                                     ? "select item,Description  from PDFFormatTable2 where amount <>0 group by item,Description having count(*)>1"
                                     : "select item,Description  from SavePDFTable where amount <>0 group by item,Description having count(*)>1";
                cnn.Open();
                ItemsDataCollection = new List<ItemsList>();
                IncreaseItemsDataCollection = new List<ItemsList>();
                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);

                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var query1 = vendor.Contains("DIP")
                    ? "select InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount from PDFFormatTable2 where amount <>0 and item = '" + ds.Tables[0].Rows[i].ItemArray[0] + "' group by InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount  order by InvoiceDate asc "
                    : "select InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount from SavePDFTable where amount <>0 and item = '" + ds.Tables[0].Rows[i].ItemArray[0] + "' group by InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount  order by InvoiceDate asc ";
                    var cmd1 = new SqlCommand(query1, cnn);
                    var sda1 = new SqlDataAdapter(cmd1);
                    var ds1 = new DataSet();

                    PercentageDataCollection = new List<ItemsList>();
                    sda1.Fill(ds1);

                    var pricecount = (from DataRow dr in ds1.Tables[0].Rows select dr.ItemArray).ToList();
                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i1 = 0; i1 < pricecount.Count - 1; i1++)
                        {
                            var percentage = Convert.ToDouble(ds1.Tables[0].Rows[i1 + 1].ItemArray[6]) -
                                             Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6]);
                            percentage = (percentage / Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6])) * 100;
                            var perce = new ItemsList
                            {
                                PercentageValue =
                                    percentage != 0 ? Convert.ToDouble(percentage.ToString("##.##")) : percentage
                            };
                            PercentageDataCollection.Add(perce);
                        }
                    }

                    var perccollection = PercentageDataCollection.Select(it => it.PercentageValue).ToList();

                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i2 = 0; i2 < pricecount.Count; i2++)
                        {
                            var d = new ItemsList
                            {
                                InvoiceNo = ds1.Tables[0].Rows[i2].ItemArray[0].ToString(),
                                InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[i2].ItemArray[1]),
                                Item = ds1.Tables[0].Rows[i2].ItemArray[2].ToString(),
                                Description = ds1.Tables[0].Rows[i2].ItemArray[3].ToString(),
                                Unit = ds1.Tables[0].Rows[i2].ItemArray[4].ToString(),
                                Shipped = ds1.Tables[0].Rows[i2].ItemArray[5].ToString(),
                                Price = Convert.ToDouble(ds1.Tables[0].Rows[i2].ItemArray[6].ToString()),
                                PercentageValue = i2 != 0 ? perccollection[i2 - 1] : 0
                            };
                            {
                                ItemsDataCollection.Add(d);
                            }
                        }
                    }

                    else if (ds1.Tables[0].Rows.Count <= 1 && ds1.Tables[0].Rows.Count != 0)
                    {
                        var d = new ItemsList
                        {
                            InvoiceNo = ds1.Tables[0].Rows[0].ItemArray[0].ToString(),
                            InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[0].ItemArray[1]),
                            Item = ds1.Tables[0].Rows[0].ItemArray[2].ToString(),
                            Description = ds1.Tables[0].Rows[0].ItemArray[3].ToString(),
                            Unit = ds1.Tables[0].Rows[0].ItemArray[4].ToString(),
                            Shipped = ds1.Tables[0].Rows[0].ItemArray[5].ToString(),
                            Price = Convert.ToDouble(ds1.Tables[0].Rows[0].ItemArray[6].ToString()),
                            PercentageValue = 0
                        };
                        {
                            ItemsDataCollection.Add(d);
                        }
                    }
                }
                var newitems =
                    ItemsDataCollection.GroupBy(i => i.Item).ToList();

                for (var i = 0; i < newitems.Count; i++)
                {
                    var decrease = newitems[i].Select(it => it.Price).ToList();

                    var initialprice = decrease.FirstOrDefault();
                    var finalprice = decrease.LastOrDefault();

                    if (initialprice < finalprice)
                    {
                        var decreselist = new ItemsList
                        {
                            InvoiceNo = newitems[i].First().InvoiceNo,
                            Description = newitems[i].First().Description,
                            Item = newitems[i].First().Item,
                            Price = newitems[i].First().Price
                        };
                        IncreaseItemsDataCollection.Add(decreselist);
                    }
                }
                return IncreaseItemsDataCollection;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IList<ItemsList> GetTop10RateDecrease(string vendor)
        {
            SqlConnection cnn;
            using (cnn = new SqlConnection(Cnn1.ConnectionString))
            {
                var query = vendor.Contains("DIP")
                                     ? "select item,Description  from PDFFormatTable2 where amount <>0 group by item,Description having count(*)>1"
                                     : "select item,Description  from SavePDFTable where amount <>0 group by item,Description having count(*)>1";
                cnn.Open();
                ItemsDataCollection = new List<ItemsList>();
                DecreaseItemsDataCollection = new List<ItemsList>();
                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);

                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var query1 = vendor.Contains("DIP")
                    ? "select InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount from PDFFormatTable2 where amount <>0 and item = '" + ds.Tables[0].Rows[i].ItemArray[0] + "' group by InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount  order by InvoiceDate asc "
                    : "select InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount from SavePDFTable where amount <>0 and item = '" + ds.Tables[0].Rows[i].ItemArray[0] + "' group by InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount  order by InvoiceDate asc ";
                    var cmd1 = new SqlCommand(query1, cnn);
                    var sda1 = new SqlDataAdapter(cmd1);
                    var ds1 = new DataSet();

                    PercentageDataCollection = new List<ItemsList>();
                    sda1.Fill(ds1);

                    var pricecount = (from DataRow dr in ds1.Tables[0].Rows select dr.ItemArray).ToList();
                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i1 = 0; i1 < pricecount.Count - 1; i1++)
                        {
                            var percentage = Convert.ToDouble(ds1.Tables[0].Rows[i1 + 1].ItemArray[6]) -
                                             Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6]);
                            percentage = (percentage / Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6])) * 100;
                            var perce = new ItemsList
                            {
                                PercentageValue =
                                    percentage != 0 ? Convert.ToDouble(percentage.ToString("##.##")) : percentage
                            };
                            PercentageDataCollection.Add(perce);
                        }
                    }

                    var perccollection = PercentageDataCollection.Select(it => it.PercentageValue).ToList();

                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i2 = 0; i2 < pricecount.Count; i2++)
                        {
                            var d = new ItemsList
                            {
                                InvoiceNo = ds1.Tables[0].Rows[i2].ItemArray[0].ToString(),
                                InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[i2].ItemArray[1]),
                                Item = ds1.Tables[0].Rows[i2].ItemArray[2].ToString(),
                                Description = ds1.Tables[0].Rows[i2].ItemArray[3].ToString(),
                                Unit = ds1.Tables[0].Rows[i2].ItemArray[4].ToString(),
                                Shipped = ds1.Tables[0].Rows[i2].ItemArray[5].ToString(),
                                Price = Convert.ToDouble(ds1.Tables[0].Rows[i2].ItemArray[6].ToString()),
                                PercentageValue = i2 != 0 ? perccollection[i2 - 1] : 0
                            };
                            {
                                ItemsDataCollection.Add(d);
                            }
                        }
                    }

                    else if (ds1.Tables[0].Rows.Count <= 1 && ds1.Tables[0].Rows.Count != 0)
                    {
                        var d = new ItemsList
                        {
                            InvoiceNo = ds1.Tables[0].Rows[0].ItemArray[0].ToString(),
                            InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[0].ItemArray[1]),
                            Item = ds1.Tables[0].Rows[0].ItemArray[2].ToString(),
                            Description = ds1.Tables[0].Rows[0].ItemArray[3].ToString(),
                            Unit = ds1.Tables[0].Rows[0].ItemArray[4].ToString(),
                            Shipped = ds1.Tables[0].Rows[0].ItemArray[5].ToString(),
                            Price = Convert.ToDouble(ds1.Tables[0].Rows[0].ItemArray[6].ToString()),
                            PercentageValue = 0
                        };
                        {
                            ItemsDataCollection.Add(d);
                        }
                    }
                }
                var newitems =
                    ItemsDataCollection.GroupBy(i => i.Item).ToList();

                for (var i = 0; i < newitems.Count; i++)
                {
                    var decrease = newitems[i].Select(it => it.Price).ToList();

                    var initialprice = decrease.FirstOrDefault();
                    var finalprice = decrease.LastOrDefault();

                    if (initialprice > finalprice)
                    {
                        var decreselist = new ItemsList
                        {
                            InvoiceNo = newitems[i].First().InvoiceNo,
                            Description = newitems[i].First().Description,
                            Item = newitems[i].First().Item,
                            Price = newitems[i].First().Price
                        };
                        DecreaseItemsDataCollection.Add(decreselist);
                    }
                }
                return DecreaseItemsDataCollection;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static List<ItemsList> GetLast2Invoices(string vendortextbox)
        {
            SqlConnection cnn;
            using (cnn = new SqlConnection(Cnn1.ConnectionString))
            {

                var query = vendortextbox.Contains("DIP")
                                     ? "select Top 2 InvoiceNo from PDFFormatTable2 where amount <>0 group by invoiceNo,InvoiceDate order by invoicedate desc"
                                     : "select Top 2 InvoiceNo from SavePDFTable where amount <>0 group by invoiceNo,InvoiceDate order by invoicedate desc";


                cnn.Open();

                var cmd = new SqlCommand(query, cnn);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                sda.Fill(ds);

                ItemsDataCollection = new List<ItemsList>();

                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var query1 = vendortextbox.Contains("DIP")
                        ? "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM PDFFormatTable2 where InvoiceNo = '" +
                          ds.Tables[0].Rows[i].ItemArray[0] + "'" + "and Amount <>0" + "order by InvoiceDate asc"
                        : "SELECT InvoiceNo,InvoiceDate,Item,Description,Unit,Shipped,Price,Amount FROM SavePDFTable where InvoiceNo = '" +
                          ds.Tables[0].Rows[i].ItemArray[0] + "'" + "and Amount <>0" + "order by InvoiceDate asc";
                    var cmd1 = new SqlCommand(query1, cnn);
                    var sda1 = new SqlDataAdapter(cmd1);
                    var ds1 = new DataSet();
                    PercentageDataCollection = new List<ItemsList>();
                    sda1.Fill(ds1);

                    var pricecount = (from DataRow dr in ds1.Tables[0].Rows select dr.ItemArray).ToList();
                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i1 = 0; i1 < pricecount.Count - 1; i1++)
                        {
                            var percentage = Convert.ToDouble(ds1.Tables[0].Rows[i1 + 1].ItemArray[6]) -
                                             Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6]);
                            percentage = (percentage / Convert.ToDouble(ds1.Tables[0].Rows[i1].ItemArray[6])) * 100;
                            var perce = new ItemsList
                            {
                                PercentageValue =
                                    percentage != 0 ? Convert.ToDouble(percentage.ToString("##.##")) : percentage
                            };
                            PercentageDataCollection.Add(perce);
                        }
                    }

                    var perccollection = PercentageDataCollection.Select(it => it.PercentageValue).ToList();

                    if (ds1.Tables[0].Rows.Count > 1)
                    {
                        for (var i2 = 0; i2 < pricecount.Count; i2++)
                        {
                            var d = new ItemsList
                            {
                                InvoiceNo = ds1.Tables[0].Rows[i2].ItemArray[0].ToString(),
                                InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[i2].ItemArray[1]),
                                Item = ds1.Tables[0].Rows[i2].ItemArray[2].ToString(),
                                Description = ds1.Tables[0].Rows[i2].ItemArray[3].ToString(),
                                Unit = ds1.Tables[0].Rows[i2].ItemArray[4].ToString(),
                                Shipped = ds1.Tables[0].Rows[i2].ItemArray[5].ToString(),
                                Price = Convert.ToDouble(ds1.Tables[0].Rows[i2].ItemArray[6].ToString()),
                                PercentageValue = i2 != 0 ? perccollection[i2 - 1] : 0
                            };
                            {
                                ItemsDataCollection.Add(d);
                            }
                        }
                    }

                    else if (ds1.Tables[0].Rows.Count <= 1 && ds1.Tables[0].Rows.Count != 0)
                    {
                        var d = new ItemsList
                        {
                            InvoiceNo = ds1.Tables[0].Rows[0].ItemArray[0].ToString(),
                            InvoiceDate = Convert.ToDateTime(ds1.Tables[0].Rows[0].ItemArray[1]),
                            Item = ds1.Tables[0].Rows[0].ItemArray[2].ToString(),
                            Description = ds1.Tables[0].Rows[0].ItemArray[3].ToString(),
                            Unit = ds1.Tables[0].Rows[0].ItemArray[4].ToString(),
                            Shipped = ds1.Tables[0].Rows[0].ItemArray[5].ToString(),
                            Price = Convert.ToDouble(ds1.Tables[0].Rows[0].ItemArray[6].ToString()),
                            PercentageValue = 0
                        };
                        {
                            ItemsDataCollection.Add(d);
                        }
                    }


                }
                return ItemsDataCollection;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static Color ColorFromName(double percentageValue)
        {
            return percentageValue > 0 ? Color.Green : Color.Red;
        }

        public static List<ItemsList> ItemsDataCollection { get; set; }

        public static List<ItemsList> PercentageDataCollection { get; set; }

        public static List<ItemsList> IncreaseItemsDataCollection { get; set; }

        public static List<ItemsList> DecreaseItemsDataCollection { get; set; }
    }

    [DataObject]
    public class ItemsList
    {
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public string Shipped { get; set; }
        public Double Price { get; set; }
        public string Amount { get; set; }
        public Double PercentageValue { get; set; }
        public string FileName { get;set; }
    }
}