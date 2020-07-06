﻿using FarmsApi.DataModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;

namespace FarmsApi.Services
{
    public class HorsesService
    {
        public static void UpdateMetaHorsses()
        {

            using (var Context = new Context())
            {
                // var Users = Context.Users.Where(x=>x.Id==-1).ToList();
                var Horses = Context.Horses.ToList();
                foreach (Horse Horsef in Horses)
                {

                    try
                    {

                        // User Horsef = Context.Users.Where(x => x.Id == 50).FirstOrDefault();yyy

                        var Meta = JObject.Parse(Horsef.Meta);

                        #region User
                        if (Meta["Active"].ToString() == "active")
                        {
                            Horsef.Active = "active";

                        }
                        else
                        {
                            Horsef.Active = "notActive";
                        }

                        if (Meta["Image"] != null)
                        {
                            Horsef.Image = Meta["Image"].ToString();

                        }
                        if (Meta["Gender"] != null)
                        {
                            Horsef.Gender = Meta["Gender"].ToString();

                        }

                        if (Meta["Ownage"] != null)
                        {
                            Horsef.Ownage = Meta["Ownage"].ToString();

                        }
                        if (Meta["BirthDate"] != null)
                        {
                            Horsef.BirthDate = CheckifExistDate(Meta["BirthDate"]);

                        }

                        if (Meta["PensionStartDate"] != null)
                        {
                            Horsef.PensionStartDate = CheckifExistDate(Meta["PensionStartDate"]);

                        }



                        if (Meta["Race"] != null)
                        {
                            Horsef.Race = Meta["Race"].ToString();

                        }
                        if (Meta["Owner"] != null)
                        {
                            Horsef.Owner = Meta["Owner"].ToString();

                        }

                        if (Meta["Father"] != null)
                        {
                            Horsef.Father = Meta["Father"].ToString();

                        }
                        if (Meta["Mother"] != null)
                        {
                            Horsef.Mother = Meta["Mother"].ToString();

                        }

                        if (Meta["Details"] != null)
                        {
                            Horsef.Details = Meta["Details"].ToString();

                        }


                        if (Meta["Food"] != null)
                        {
                            var Food = JObject.Parse(Meta["Food"].ToString());

                            if (Food["Morning1"] != null)
                            {
                                Horsef.Morning1 = Food["Morning1"].ToString();
                            }

                            if (Food["Morning2"] != null)
                            {
                                Horsef.Morning2 = Food["Morning2"].ToString();
                            }

                            if (Food["Lunch1"] != null)
                            {
                                Horsef.Lunch1 = Food["Lunch1"].ToString();
                            }

                            if (Food["Lunch2"] != null)
                            {
                                Horsef.Lunch2 = Food["Lunch2"].ToString();
                            }

                            if (Food["Dinner1"] != null)
                            {
                                Horsef.Dinner1 = Food["Dinner1"].ToString();
                            }

                            if (Food["Dinner2"] != null)
                            {
                                Horsef.Dinner2 = Food["Dinner2"].ToString();
                            }

                        }








                        Context.Entry(Horsef).State = System.Data.Entity.EntityState.Modified;


                        #endregion

                        //**************************************************************************
                        if (Meta["Treatments"] != null)
                        {
                            foreach (var Item in Meta["Treatments"])
                            {

                                HorseTreatments hs = new HorseTreatments();

                                hs.HorseId = Horsef.Id;
                                hs.Date = CheckifExistDate(Item["Date"]);
                                hs.Name = Item["Details"].ToString();


                                Context.HorseTreatments.Add(hs);
                                // Context.Entry(pay).State = System.Data.Entity.EntityState.Added;
                                //    Context.SaveChanges();

                                // Item["resourceId"] = Horsef.Id;
                            }
                        }


                        if (Meta["Vaccinations"] != null)
                        {
                            foreach (var Item in Meta["Vaccinations"])
                            {

                                HorseVaccinations hs = new HorseVaccinations();

                                hs.HorseId = Horsef.Id;
                                hs.Date = CheckifExistDate(Item["Date"]);
                                hs.Type = Item["Type"].ToString();
                                hs.Name = Item["Details"].ToString();


                                Context.HorseVaccinations.Add(hs);

                            }
                        }


                        if (Meta["Shoeings"] != null)
                        {
                            foreach (var Item in Meta["Shoeings"])
                            {

                                HorseShoeings hs = new HorseShoeings();

                                hs.HorseId = Horsef.Id;
                                hs.Date = CheckifExistDate(Item["Date"]);
                                hs.ShoerName = Item["ShoerName"].ToString();
                                hs.Name = Item["Details"].ToString();
                                hs.IsPaid = CheckifExistBool(Item["Paid"].ToString());

                                Context.HorseShoeings.Add(hs);

                            }
                        }

                        if (Meta["Tilufings"] != null)
                        {
                            foreach (var Item in Meta["Tilufings"])
                            {



                                HorseTilufings hs = new HorseTilufings();

                                hs.HorseId = Horsef.Id;
                                hs.Date = CheckifExistDate(Item["Date"]);
                                hs.ShoerName = Item["ShoerName"].ToString();
                                hs.Name = Item["Details"].ToString();
                                hs.IsPaid = CheckifExistBool(Item["Paid"].ToString());

                                Context.HorseTilufings.Add(hs);



                            }
                        }

                        if (Meta["Pregnancies"] != null)
                        {
                            foreach (var Item in Meta["Pregnancies"])
                            {



                                HorsePregnancies hs = new HorsePregnancies();

                                hs.HorseId = Horsef.Id;
                                hs.Date = CheckifExistDate(Item["Date"]);
                                hs.Father = CheckifExistStr(Item["Father"]);
                                hs.Comments = CheckifExistStr(Item["Details"]);
                                hs.IsSurrogate = CheckifExistBool(Item["IsSurrogate"]);
                                hs.Finished = CheckifExistBool(Item["Finished"]);

                                if (Item["Surrogate"] != null)
                                {
                                    var Surrogate = JObject.Parse(Item["Surrogate"].ToString());
                                    hs.SurrogateId = CheckifExistInt(Surrogate["Id"]);
                                    hs.SurrogateName = Surrogate["Name"].ToString();

                                }

                                Context.HorsePregnancies.Add(hs);
                                Context.SaveChanges();

                                if (Item["States"] != null)
                                {

                                    //  var States = JObject.Parse(Item["States"].ToString());
                                    foreach (var st in Item["States"])
                                    {


                                        var State = JObject.Parse(st["State"].ToString());

                                        HorsePregnanciesStates hss = new HorsePregnanciesStates();
                                        hss.Date = CheckifExistDate(Item["Date"]);
                                        hss.HorseId = Horsef.Id;
                                        hss.HorsePregnanciesId = hs.Id;
                                        hss.StateId = State["id"].ToString();
                                        hss.name = State["name"].ToString();
                                        hss.day = State["day"].ToString();

                                        Context.HorsePregnanciesStates.Add(hss);

                                    }

                                }

                            }
                        }




                        #region Temp
                        //#region Payments
                        //if (Meta["Payments"] != null)
                        //{
                        //    foreach (var Item in Meta["Payments"])
                        //    {

                        //        Payments pay = new Payments();

                        //        pay.UserId = Horsef.Id;
                        //        pay.Date = CheckifExistDate(Item["Date"]);
                        //        pay.InvoicePdf = CheckifExistStr(Item["InvoicePdf"]);
                        //        pay.InvoiceNum = CheckifExistStr(Item["InvoiceNum"]);
                        //        pay.InvoiceDetails = CheckifExistStr(Item["InvoiceDetails"]);

                        //        pay.canceled = CheckifExistStr(Item["canceled"]);
                        //        pay.Price = CheckifExistDouble(Item["Price"]);
                        //        pay.InvoiceSum = CheckifExistDouble(Item["InvoiceSum"]);

                        //        pay.payment_type = CheckifExistStr(Item["payment_type"]);
                        //        pay.lessons = CheckifExistInt(Item["lessons"]);
                        //        pay.month = CheckifExistDate(Item["month"]);
                        //        pay.untilmonth = CheckifExistDate(Item["untilmonth"]);


                        //        Context.Payments.Add(pay);
                        //        // Context.Entry(pay).State = System.Data.Entity.EntityState.Added;
                        //        //    Context.SaveChanges();

                        //        // Item["resourceId"] = Horsef.Id;
                        //    }
                        //}

                        //#endregion

                        //#region Expenses
                        //if (Meta["Expenses"] != null)
                        //{
                        //    foreach (var Item in Meta["Expenses"])
                        //    {
                        //        Expenses Exp = new Expenses();
                        //        Exp.UserId = Horsef.Id;
                        //        Exp.Date = CheckifExistDate(Item["Date"]);
                        //        Exp.Price = CheckifExistDouble(Item["Price"]);
                        //        Exp.Details = CheckifExistStr(Item["Details"]);
                        //        Exp.Paid = CheckifExistStr(Item["Paid"]);

                        //        Context.Expenses.Add(Exp);
                        //        //   Context.SaveChanges();
                        //        // Item["resourceId"] = Horsef.Id;
                        //    }
                        //}

                        //#endregion

                        //#region AvailableHours
                        //if (Meta["AvailableHours"] != null)
                        //{
                        //    foreach (var Item in Meta["AvailableHours"])
                        //    {
                        //        AvailableHours Exp = new AvailableHours();
                        //        Exp.UserId = Horsef.Id;
                        //        Exp.resourceId = CheckifExistInt(Item["resourceId"]);
                        //        Exp.start = CheckifExistStr(Item["start"]);
                        //        Exp.end = CheckifExistStr(Item["end"]);

                        //        string res = (Item["dow"].ToString()).Replace("[", "").Replace("]", "").Replace("\"", "").Trim();

                        //        //  string res = Regex.Replace(Item["dow"].ToString(), "\"[^\"]*\"", string.Empty);
                        //        Exp.dow = res;//CheckifExistStr(CheckifExistInt(res));

                        //        Context.AvailableHours.Add(Exp);
                        //        //   Context.SaveChanges();
                        //        // Item["resourceId"] = Horsef.Id;
                        //    }
                        //}

                        //#endregion

                        //#region Commitments
                        //if (Meta["Commitments"] != null)
                        //{
                        //    foreach (var Item in Meta["Commitments"])
                        //    {
                        //        Commitments Com = new Commitments();
                        //        Com.UserId = Horsef.Id;
                        //        Com.Date = (CheckifExistDate(Item["Date"]) == null) ? new DateTime(2016, 01, 01) : CheckifExistDate(Item["Date"]);
                        //        Com.Price = CheckifExistStr(Item["Price"]);

                        //        Com.HMO = CheckifExistStr(Item["HMO"]);
                        //        Com.Qty = CheckifExistDouble(Item["Qty"]);
                        //        Com.Number = CheckifExistStr(Item["Number"]);
                        //        Com.canceled = CheckifExistStr(Item["canceled"]);
                        //        Com.InvoiceSum = CheckifExistStr(Item["InvoiceSum"]);

                        //        Context.Commitments.Add(Com);
                        //        //  Context.SaveChanges();
                        //        Item["resourceId"] = Horsef.Id;
                        //    }
                        //}

                        //#endregion

                        //#region Horses
                        //if (Meta["Horses"] != null)
                        //{
                        //    foreach (var Item in Meta["Horses"])
                        //    {
                        //        UserHorses Uh = new UserHorses();
                        //        Uh.UserId = Horsef.Id;
                        //        Uh.Name = CheckifExistStr(Item["Name"]);
                        //        Uh.Owner = CheckifExistBool(Item["Owner"]);

                        //        Uh.PensionPrice = CheckifExistInt(Item["PensionPrice"]);
                        //        Uh.TrainingCost = CheckifExistInt(Item["TrainingCost"]);


                        //        Context.UserHorses.Add(Uh);
                        //        //         Context.SaveChanges();
                        //        // Item["resourceId"] = Horsef.Id;
                        //    }
                        //}

                        //#endregion

                        //#region Files
                        //if (Meta["Files"] != null)
                        //{
                        //    foreach (var Item in Meta["Files"])
                        //    {
                        //        Files Fi = new Files();
                        //        Fi.UserId = Horsef.Id;
                        //        Fi.Link = CheckifExistStr(Item);

                        //        Context.Files.Add(Fi);
                        //        //    Context.SaveChanges();
                        //        // Item["resourceId"] = Horsef.Id;
                        //    }
                        //}

                        //#endregion
                        #endregion
                        Context.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            //Debug.WriteLine(@"Entity of type ""{0}"" in state ""{1}"" 
                            // has the following validation errors:",
                            //eve.Entry.Entity.GetType().Name,
                            // eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                // Debug.WriteLine(@"- Property: ""{0}"", Error: ""{1}""",
                                //  ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    catch (DbUpdateException e)
                    {
                        //Add your code to inspect the inner exception and/or
                        //e.Entries here.
                        //Or just use the debugger.
                        //Added this catch (after the comments below) to make it more obvious 
                        //how this code might help this specific problem
                    }
                    catch (Exception e)
                    {

                    }
                }

            }


        }


        private static double? CheckifExistDouble(JToken jToken)
        {
            if (jToken == null) return 0;
            double res;
            bool Ok = Double.TryParse(jToken.ToString(), out res);
            if (Ok) return res;
            else return 0;
        }

        private static bool CheckifExistBool(JToken jToken)
        {
            if (jToken == null) return false;
            Boolean res = new Boolean();
            bool Ok = Boolean.TryParse(jToken.ToString(), out res);
            if (Ok)
                return res;
            else
                return false;
        }

        private static DateTime? CheckifExistDate(JToken jToken)
        {

            if (jToken == null) return null;
            DateTime res = new DateTime();
            bool Ok = DateTime.TryParse(jToken.ToString(), out res);
            if (Ok && res.Year > 1960) return res;

            else return null;
        }

        private static int CheckifExistInt(JToken jToken)
        {
            if (jToken == null) return 0;
            int res;
            bool Ok = Int32.TryParse(jToken.ToString(), out res);
            if (Ok) return res;
            else return 0;
        }

        private static string CheckifExistStr(JToken jToken)
        {

            if (jToken != null)

                return jToken.ToString();

            return "";

        }


        public static Horse UpdateHorseMultiTables(JArray dataObj)
        {


            Horse h = UpdateHorse(dataObj[0].ToObject<Horse>());

            List<HorseFiles> f = dataObj[1].ToObject<List<HorseFiles>>();
            UpdateHorseFilesObject(f, h);

            List<HorseHozeFiles> hf = dataObj[2].ToObject<List<HorseHozeFiles>>();
            UpdateHorseHozeFilesObject(hf, h);

            List<HorsePundekautFiles> pf = dataObj[3].ToObject<List<HorsePundekautFiles>>();
            UpdateHorsePundekautFilesObject(pf, h);




            List<HorseTreatments> ht = dataObj[4].ToObject<List<HorseTreatments>>();
            UpdateHorseTreatmentsObject(ht, h);

            List<HorseVaccinations> hv = dataObj[5].ToObject<List<HorseVaccinations>>();
            UpdateHorseVaccinationsObject(hv, h);

            List<HorseShoeings> hs = dataObj[6].ToObject<List<HorseShoeings>>();
            UpdateHorseShoeingsObject(hs, h);

            List<HorseTilufings> htl = dataObj[7].ToObject<List<HorseTilufings>>();
            UpdateHorseTilufingsObject(htl, h);

            List<HorsePregnancies> htp = dataObj[8].ToObject<List<HorsePregnancies>>();
            UpdateHorsePregnanciesObject(htp, h);
            List<HorsePregnanciesStates> htps = dataObj[9].ToObject<List<HorsePregnanciesStates>>();
            UpdateHorsePregnanciesStatesObject(htps, h);

            List<HorseInseminations> hti = dataObj[10].ToObject<List<HorseInseminations>>();
            UpdateHorseInseminationsObject(hti, h);

            return h;
        }



        private static void UpdateHorseFilesObject(List<HorseFiles> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorseFiles item in objList)
                {

                    item.HorseId = f.Id;

                    if (item.Id == 0)
                    {
                        Context.HorseFiles.Add(item);

                    }
                    else
                    {

                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        //  Context.SaveChanges();

                    }

                }

                try
                {

                    var result = Context.HorseFiles.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorseFiles> differenceQuery = result.Except(objList);

                    foreach (HorseFiles item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }

        private static void UpdateHorseHozeFilesObject(List<HorseHozeFiles> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorseHozeFiles item in objList)
                {

                    item.HorseId = f.Id;

                    if (item.Id == 0)
                    {
                        Context.HorseHozeFiles.Add(item);

                    }
                    else
                    {

                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        //  Context.SaveChanges();

                    }

                }

                try
                {

                    var result = Context.HorseHozeFiles.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorseHozeFiles> differenceQuery = result.Except(objList);

                    foreach (HorseHozeFiles item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }

        private static void UpdateHorsePundekautFilesObject(List<HorsePundekautFiles> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorsePundekautFiles item in objList)
                {

                    item.HorseId = f.Id;

                    if (item.Id == 0)
                    {
                        Context.HorsePundekautFiles.Add(item);

                    }
                    else
                    {

                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        //  Context.SaveChanges();

                    }

                }

                try
                {

                    var result = Context.HorsePundekautFiles.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorsePundekautFiles> differenceQuery = result.Except(objList);

                    foreach (HorsePundekautFiles item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }


        private static void UpdateHorseTreatmentsObject(List<HorseTreatments> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorseTreatments item in objList)
                {

                    item.HorseId = f.Id;

                    if (item.Id == 0)
                    {
                        int? ExpensesId = AddToExpensesTable(item.Cost, item.Discount, item.HorseId, f.Name, item.Name, item.Date);
                        item.ExpensesId = ExpensesId;
                        Context.HorseTreatments.Add(item);

                    }
                    else
                    {

                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        //  Context.SaveChanges();

                    }

                }

                try
                {

                    var result = Context.HorseTreatments.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorseTreatments> differenceQuery = result.Except(objList);

                    foreach (HorseTreatments item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }

        private static int? AddToExpensesTable(double? cost, double? discount, int horseId, string horsename, string name, DateTime? date)
        {


            DateTime dt = (DateTime)date;

            int? res = null;
            using (var Context = new Context())
            {

                UserHorses uh = Context.UserHorses.SingleOrDefault(u => u.HorseId == horseId && u.Owner);

                if (uh != null)
                {



                    Expenses e = new Expenses();
                    e.Details = " תשלום עבור " + name + " לסוס " + horsename + " (" + horseId.ToString() + ")  בתאריך " + dt.ToString("dd/MM/yy");
                    e.Price = (cost ?? 0) - (discount ?? 0);
                    e.UserId = uh.UserId;
                    e.Date = date;
                    Context.Expenses.Add(e);
                    Context.SaveChanges();

                    res = e.Id;
                }

                return res;

            }
        }

        private static void UpdateHorseVaccinationsObject(List<HorseVaccinations> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorseVaccinations item in objList)
                {

                    item.HorseId = f.Id;

                    if (item.Id == 0)
                    {
                        int? ExpensesId = AddToExpensesTable(item.Cost, item.Discount, item.HorseId, f.Name, item.Name, item.Date);
                        item.ExpensesId = ExpensesId;
                        Context.HorseVaccinations.Add(item);

                    }
                    else
                    {

                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        //  Context.SaveChanges();

                    }

                }

                try
                {

                    var result = Context.HorseVaccinations.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorseVaccinations> differenceQuery = result.Except(objList);

                    foreach (HorseVaccinations item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }

        private static void UpdateHorseShoeingsObject(List<HorseShoeings> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorseShoeings item in objList)
                {

                    item.HorseId = f.Id;

                    if (item.Id == 0)
                    {
                        int? ExpensesId = AddToExpensesTable(item.Cost, item.Discount, item.HorseId, f.Name, item.Name, item.Date);
                        item.ExpensesId = ExpensesId;
                        Context.HorseShoeings.Add(item);

                    }
                    else
                    {

                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        //  Context.SaveChanges();

                    }

                }

                try
                {

                    var result = Context.HorseShoeings.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorseShoeings> differenceQuery = result.Except(objList);

                    foreach (HorseShoeings item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }

        private static void UpdateHorseTilufingsObject(List<HorseTilufings> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorseTilufings item in objList)
                {

                    item.HorseId = f.Id;

                    if (item.Id == 0)
                    {
                        int? ExpensesId = AddToExpensesTable(item.Cost, item.Discount, item.HorseId, f.Name, item.Name, item.Date);
                        item.ExpensesId = ExpensesId;
                        Context.HorseTilufings.Add(item);

                    }
                    else
                    {

                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        //  Context.SaveChanges();

                    }

                }

                try
                {

                    var result = Context.HorseTilufings.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorseTilufings> differenceQuery = result.Except(objList);

                    foreach (HorseTilufings item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }


        private static void UpdateHorsePregnanciesObject(List<HorsePregnancies> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorsePregnancies item in objList)
                {
                    item.HorseId = f.Id;
                    if (item.Id == 0)
                    {
                        Context.HorsePregnancies.Add(item);




                    }
                    else
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }

                }

                try
                {

                    var result = Context.HorsePregnancies.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorsePregnancies> differenceQuery = result.Except(objList);

                    foreach (HorsePregnancies item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }

        private static void UpdateHorsePregnanciesStatesObject(List<HorsePregnanciesStates> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorsePregnanciesStates item in objList)
                {

                    item.HorseId = f.Id;

                    if (item.Id == 0)
                    {

                        if (item.StateId == "insemination")
                        {

                            HorseInseminations LastInseminations = Context.HorseInseminations.Where(h => h.PregnanciesHorseId == item.HorseId).OrderByDescending(y => y.InseminationDate).FirstOrDefault();
                            if (LastInseminations != null)
                            {
                                LastInseminations.InseminationDate = item.Date;
                                Context.Entry(LastInseminations).State = System.Data.Entity.EntityState.Modified;
                            }

                        }


                        if (item.StateId == "ultrasound1")
                        {

                            HorseInseminations LastInseminations = Context.HorseInseminations.Where(h => h.PregnanciesHorseId == item.HorseId).OrderByDescending(y => y.InseminationDate).FirstOrDefault();
                            if (LastInseminations != null)
                            {
                                LastInseminations.HerionDate = item.Date;

                                var dateInseminations = Context.HorsePregnanciesStates.Where(x => x.HorsePregnanciesId == item.HorsePregnanciesId && x.StateId == "insemination").FirstOrDefault();
                                if (dateInseminations != null)
                                {
                                    LastInseminations.LedaDate = ((DateTime)dateInseminations.Date).AddMonths(11);
                                }

                                Context.Entry(LastInseminations).State = System.Data.Entity.EntityState.Modified;
                            }



                        }



                        Context.HorsePregnanciesStates.Add(item);

                    }
                    else
                    {

                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        //  Context.SaveChanges();

                    }

                }

                try
                {

                    var result = Context.HorsePregnanciesStates.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorsePregnanciesStates> differenceQuery = result.Except(objList);

                    foreach (HorsePregnanciesStates item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }



        private static void UpdateHorseInseminationsObject(List<HorseInseminations> objList, Horse f)
        {
            using (var Context = new Context())
            {

                foreach (HorseInseminations item in objList)
                {

                    item.HorseId = f.Id;

                    if (item.Id == 0)
                    {
                        Context.HorseInseminations.Add(item);

                    }
                    else
                    {

                        Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        //  Context.SaveChanges();

                    }

                }

                try
                {

                    var result = Context.HorseInseminations.Where(p => p.HorseId == f.Id).ToList();
                    IEnumerable<HorseInseminations> differenceQuery = result.Except(objList);

                    foreach (HorseInseminations item in differenceQuery)
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }



                }
                catch (Exception ex)
                {


                }

                Context.SaveChanges();


            }
        }

        public static List<Horse> GetSusut()
        {
            using (var Context = new Context())
            {
                var CurrentHorsefarmId = UsersService.GetCurrentUser().Farm_Id;

                SqlParameter Farm_IdPara = new SqlParameter("Farm_Id", CurrentHorsefarmId);

                var query = Context.Database.SqlQuery<Horse>
                ("GetSusut @Farm_Id", Farm_IdPara);



                var susut = query.ToList();


                return susut;
            }
        }

        public static List<Horse> GetHorses(bool IncludeDeleted = false)
        {
            using (var Context = new Context())
            {
                var CurrentHorsefarmId = UsersService.GetCurrentUser().Farm_Id;
                var Horses = Context.Horses.ToList();
                if (CurrentHorsefarmId != 0)
                {
                    Horses = Horses.Where(h => h.Farm_Id == CurrentHorsefarmId).OrderBy(x => x.Name).ToList();
                }
                Horses = FilterDeleted(Horses, IncludeDeleted);
                return Horses;
            }
        }

        private static List<Horse> FilterDeleted(List<Horse> Horses, bool IncludeDeleted)
        {
            if (IncludeDeleted)
            {
                return Horses;
            }

            return Horses.Where(u => !u.Deleted).ToList();
        }

        public static Horse GetHorse(int Id)
        {
            using (var Context = new Context())
                return Context.Horses.SingleOrDefault(u => u.Id == Id && !u.Deleted);
        }

        public static List<HorseFiles> GetHorseFiles(int Id)
        {
            using (var Context = new Context())
                return Context.HorseFiles.Where(u => u.HorseId == Id).ToList();
        }

        public static List<HorseHozeFiles> GetHorseHozeFiles(int Id)
        {
            using (var Context = new Context())
                return Context.HorseHozeFiles.Where(u => u.HorseId == Id).ToList();
        }
        public static List<HorsePundekautFiles> GetHorsePundekautFiles(int Id)
        {
            using (var Context = new Context())
                return Context.HorsePundekautFiles.Where(u => u.HorseId == Id).ToList();
        }


        public static List<HorseTreatments> GetHorseTreatments(int Id)
        {
            using (var Context = new Context())
            {
                SqlParameter TypePara = new SqlParameter("Type", 1);
                SqlParameter HorseIdPara = new SqlParameter("HorseId", Id);
                var query = Context.Database.SqlQuery<HorseTreatments>
                ("GetHorseObject  @Type,@HorseId", TypePara, HorseIdPara);
                var Objects = query.ToList();
                return Objects;

            }

        }

        public static List<HorseVaccinations> GetHorseVaccinations(int Id)
        {
            using (var Context = new Context())
            {
                SqlParameter TypePara = new SqlParameter("Type", 2);
                SqlParameter HorseIdPara = new SqlParameter("HorseId", Id);
                var query = Context.Database.SqlQuery<HorseVaccinations>
                ("GetHorseObject  @Type,@HorseId", TypePara, HorseIdPara);
                var Objects = query.ToList();
                return Objects;

            }


            //using (var Context = new Context())
            //    return Context.HorseVaccinations.Where(u => u.HorseId == Id).ToList();
        }

        public static List<HorseShoeings> GetHorseShoeings(int Id)
        {
            using (var Context = new Context())
            {
                SqlParameter TypePara = new SqlParameter("Type", 3);
                SqlParameter HorseIdPara = new SqlParameter("HorseId", Id);
                var query = Context.Database.SqlQuery<HorseShoeings>
                ("GetHorseObject  @Type,@HorseId", TypePara, HorseIdPara);
                var Objects = query.ToList();
                return Objects;

            }
            //using (var Context = new Context())
            //    return Context.HorseShoeings.Where(u => u.HorseId == Id).ToList();
        }

        public static List<HorseTilufings> GetHorseTilufings(int Id)
        {
            using (var Context = new Context())
            {
                SqlParameter TypePara = new SqlParameter("Type", 4);
                SqlParameter HorseIdPara = new SqlParameter("HorseId", Id);
                var query = Context.Database.SqlQuery<HorseTilufings>
                ("GetHorseObject  @Type,@HorseId", TypePara, HorseIdPara);
                var Objects = query.ToList();
                return Objects;

            }
            //using (var Context = new Context())
            //    return Context.HorseTilufings.Where(u => u.HorseId == Id).ToList();
        }
        public static List<HorsePregnancies> GetHorsePregnancies(int Id)
        {
            using (var Context = new Context())
                return Context.HorsePregnancies.Where(u => u.HorseId == Id).ToList();
        }

        public static List<HorsePregnanciesStates> GetHorsePregnanciesStates(int Id)
        {
            using (var Context = new Context())
                return Context.HorsePregnanciesStates.Where(u => u.HorseId == Id).ToList();
        }
        public static List<HorseInseminationsResult> GetHorseInseminations(int Id)
        {
            using (var Context = new Context())
            {

                SqlParameter HorseIdPara = new SqlParameter("HorseId", Id);

                var query = Context.Database.SqlQuery<HorseInseminationsResult>
                ("GetHorseInseminations @HorseId", HorseIdPara);



                var Inseminations = query.ToList();

                return Inseminations;


            }
            // return Context.HorseInseminations.Where(u => u.HorseId == Id).ToList();
        }

        public static HorsePregnancies InsertNewPregnancie(bool isBuild, HorsePregnancies hp)
        {
            using (var Context = new Context())
            {
                var CurrentHorsefarmId = UsersService.GetCurrentUser().Farm_Id;


                Context.HorsePregnancies.Add(hp);
                Context.SaveChanges();

                if (isBuild)
                {
                    HorsePregnanciesStates hh = new HorsePregnanciesStates();
                    hh.HorseId = hp.HorseId;
                    hh.HorsePregnanciesId = hp.Id;
                    hh.Date = hp.Date;
                    hh.StateId = "implantation";
                    hh.name = "השתלה";

                    Context.HorsePregnanciesStates.Add(hh);
                    Context.SaveChanges();

                }



                return hp;
            }
        }



        public static Horse UpdateHorse(Horse Horse)
        {
            using (var Context = new Context())
            {
                var CurrentHorsefarmId = UsersService.GetCurrentUser().Farm_Id;
                Horse.Farm_Id = CurrentHorsefarmId != 0 ? CurrentHorsefarmId : Horse.Farm_Id;
                if (Horse.Id == 0)
                    Context.Horses.Add(Horse);
                else
                    Context.Entry(Horse).State = System.Data.Entity.EntityState.Modified;

                Context.SaveChanges();
                return Horse;
            }
        }

        public static int CheckIfHorseWork(int? id, string start, string end)
        {
            using (var Context = new Context())
            {
                try
                {

                    DateTime dtStart = Convert.ToDateTime(start);
                    DateTime dtSEnd = Convert.ToDateTime(end);

                    var isExist = (from l in Context.Lessons
                                   join s in Context.StudentLessons
                                   on l.Id equals s.Lesson_Id
                                   where s.HorseId == id &&
                                   ((l.Start >= dtStart && l.Start < dtSEnd) || (dtStart >= l.Start && dtStart < l.End))
                                   select new
                                   {
                                       ID = s.HorseId

                                   }).ToList();


                    return isExist.Count();


                }
                catch (Exception ex)
                {


                }


            }

            return 0;
        }
        public static void DeleteHorse(int Id)
        {
            using (var Context = new Context())
            {
                var Horse = Context.Horses.SingleOrDefault(u => u.Id == Id);
                var HorseNotifications = Context.Notifications.Where(n => n.EntityType == "horse" && n.EntityId == Horse.Id);
                if (Horse != null)
                {
                    Horse.Deleted = true;
                    Context.Notifications.RemoveRange(HorseNotifications);
                }
                Context.SaveChanges();
            }
        }
    }
}