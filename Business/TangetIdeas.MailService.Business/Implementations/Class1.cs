//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TangetIdeas.MailService.Business.Implementations
//{
//    class Class1
//    {
//        public static bool MusteriRevizeFinalMailiGonder(int Orderid)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                //msg.To.Add(new MailAddress("bykezza@gmail.com"));
//                msg.To.Add(new MailAddress(ord.aspnet_User.LoweredUserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = Orderid + " Numaralı Sipariş Revize Final Dosyası";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/revize-final.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                string KurumAdi = "";
//                if (ord.KurumsalisMi == true)
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(ord.UserId.ToString()));

//                body = body.Replace("{tabloYukle}", Helper.YonetimIcinRevizeFinalDosyalariTablosuOlustur(ord));
//                string DokumanTipi;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                }

//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));



//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());



//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);
//                DateTime RevizeTarihi = Convert.ToDateTime(ord.Revizes.FirstOrDefault().RevizeTarihSaati);
//                body = body.Replace("{RevizeTarihi}", RevizeTarihi.ToShortDateString() + RevizeTarihi.ToShortTimeString());
//                body = body.Replace("{RevizeYorumu}", ord.Revizes.FirstOrDefault().RevizeYorumu);

//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//        public static bool MusteriRevizeGonderdiMailiGonder(int Orderid)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                //msg.To.Add(new MailAddress("bykezza@gmail.com"));
//                msg.To.Add(new MailAddress(ord.aspnet_User.LoweredUserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = Orderid + " Numaralı Sipariş Revize Talebi";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/musteri-revize-gonder.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                string KurumAdi = "";
//                if (ord.KurumsalisMi == true)
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(ord.UserId.ToString()));

//                body = body.Replace("{tabloYukle}", Helper.YonetimIcinRevizeDosyalarTablosuOlustur(ord));
//                string DokumanTipi;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                }

//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));



//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());



//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);
//                DateTime RevizeTarihi = Convert.ToDateTime(ord.Revizes.FirstOrDefault().RevizeTarihSaati);
//                body = body.Replace("{RevizeTarihi}", RevizeTarihi.ToShortDateString() + RevizeTarihi.ToShortTimeString());
//                body = body.Replace("{RevizeYorumu}", ord.Revizes.FirstOrDefault().RevizeYorumu);

//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//        public static void TercumanOdemesiYapildiMailiGonder(int TercumanIsId)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                Order_Tercuman ot = db.Order_Tercumans.Where(a => a.TercumanIsId == TercumanIsId).FirstOrDefault();
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.To.Add(new MailAddress(ot.aspnet_User.LoweredUserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = "Çeviri Dükkanından Ödeme Gerçekleşti";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/dekont.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                DateTime odemeTarihi = (DateTime)ot.OdemeninYapildigiTarihi;

//                body = body.Replace("{odemeTarihi}", String.Format("{0:0000}", odemeTarihi.Year.ToString()) + "/" + String.Format("{0:00}", odemeTarihi.Month) + "/" + String.Format("{0:00}", odemeTarihi.Day) + " " + String.Format("{0:00}", odemeTarihi.Hour) + ":" + String.Format("{0:00}", odemeTarihi.Minute));
//                body = body.Replace("{orderid}", ot.Dosya.OrderId.ToString());

//                body = body.Replace("{alacakliAdSoyad}", Helper.AdSoyadGetir(ot.TercumanId.ToString()));
//                string iban = "", banka = "", acctype = "";
//                TercumanOdemeBilgileri tob = db.TercumanOdemeBilgileris.Where(a => a.TercumanId == ot.TercumanId).FirstOrDefault();
//                if (tob != null)
//                {
//                    if (tob.AccountType.ToLower() == "turkish")
//                    {
//                        acctype = "Turkish Account";
//                        banka = tob.TRBankName + " - " + tob.TRAccountHolder;
//                        iban = tob.TRIBAN;
//                    }
//                    else if (tob.AccountType.ToLower() == "european")
//                    {
//                        acctype = "European Account";
//                        banka = tob.EUIBAN;
//                        iban = tob.EUBankName;
//                    }
//                    else
//                    {
//                        acctype = "Paypal";
//                        banka = "Paypal";
//                        iban = tob.PaypalEmailAddress;
//                    }
//                }
//                body = body.Replace("{alacakliIBAN}", acctype + " - " + banka + " - " + iban);
//                decimal TercumanaOdenecekTutar = (decimal)ot.TercumanaOdenecekMiktar;
//                body = body.Replace("{odenmisTutar}", String.Format("{0:0.00}", TercumanaOdenecekTutar));
//                msg.Body = body;
//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                client.Send(msg);
//            }

//        }
//        public static void AdmineTestAlindiMailiGonder(int cevapid)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                TercumanTestCevap ttc = db.TercumanTestCevaps.Where(a => a.TercumanCevapId == cevapid).FirstOrDefault();
//                string adSoyad = Helper.AdSoyadGetir(ttc.UserId.ToString());
//                string eposta = Membership.GetUser((Guid)ttc.UserId).UserName;
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");

//                msg.To.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.To.Add(new MailAddress("cansu@ceviridukkani.com"));
//                msg.To.Add(new MailAddress("candice@ceviridukkani.com"));
//                msg.To.Add(new MailAddress("hr@ceviridukkani.com"));
//                msg.To.Add(new MailAddress("editor@ceviridukkani.com"));



//                msg.Subject = "Test Tercüme Kontrolü";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/Admin-Test-Onayla.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{adsoyad}", adSoyad);
//                body = body.Replace("{kaynakdil}", Helper.DilGetir((int)ttc.TercumanTest.TestKaynakDili));
//                body = body.Replace("{hedefdil}", Helper.DilGetir((int)ttc.HedefDil));
//                body = body.Replace("{kaynakDokuman}", "<img src=\"http://ceviridukkani.com/" + ttc.TercumanTest.TestResmi + "\" />");
//                body = body.Replace("{tercumesi}", ttc.Cevap);
//                body = body.Replace("{tarih}", ttc.CevaplamaTarihi.ToString());
//                body = body.Replace("{testcevapid}", cevapid.ToString());



//                msg.Body = body;
//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                client.Send(msg);
//            }
//        }
//        public static void TestAlindiMailiGonder(int cevapid)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                TercumanTestCevap ttc = db.TercumanTestCevaps.Where(a => a.TercumanCevapId == cevapid).FirstOrDefault();
//                string adSoyad = Helper.AdSoyadGetir(ttc.UserId.ToString());
//                string eposta = Membership.GetUser((Guid)ttc.UserId).UserName;
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.To.Add(new MailAddress(eposta));
//                msg.Subject = "Test Tercümeniz Alındı";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/Test-Gonder.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{adsoyad}", adSoyad);
//                body = body.Replace("{kaynakdil}", Helper.DilGetir((int)ttc.TercumanTest.TestKaynakDili));
//                body = body.Replace("{hedefdil}", Helper.DilGetir((int)ttc.HedefDil));
//                body = body.Replace("{kaynakDokuman}", "<img src=\"http://ceviridukkani.com/" + ttc.TercumanTest.TestResmi + "\" />");
//                body = body.Replace("{tercumesi}", ttc.Cevap);
//                body = body.Replace("{testcevapid}", cevapid.ToString());

//                msg.Body = body;
//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                client.Send(msg);
//            }
//        }
//        public static void EpostaOnayiMailiGonder(Guid userid)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                string adSoyad = Helper.AdSoyadGetir(userid.ToString());
//                string eposta = Membership.GetUser(userid).UserName;
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.To.Add(new MailAddress(eposta));
//                msg.Subject = "Eposta Adresinizi Doğrulayın";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/email-confirmation.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{adsoyad}", adSoyad);
//                body = body.Replace("{userid}", userid.ToString());

//                msg.Body = body;
//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                client.Send(msg);
//            }
//        }
//        public static void TercumanHosgeldinMailiGonder(Guid userid)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                string adSoyad = Helper.AdSoyadGetir(userid.ToString());
//                string eposta = Membership.GetUser(userid).UserName;
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                // msg.Bcc.Add(new MailAddress("hr@ceviridukkani.com"));
//                msg.To.Add(new MailAddress(eposta));
//                msg.Subject = "Hoşgeldiniz";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/Translator-Welcoming-Mail.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{adsoyad}", adSoyad);
//                msg.Body = body;
//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                client.Send(msg);
//            }
//        }
//        public static void KurumsalKullaniciHosgeldinMailiGonder(Guid userid)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                string adSoyad = Helper.AdSoyadGetir(userid.ToString());
//                string SirketAdi = Helper.KurumAdiGetir(userid);
//                string KurumKodu = Helper.GetKurumIdByUserId(userid.ToString()).ToString();
//                string eposta = Membership.GetUser(userid).UserName;
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.To.Add(new MailAddress(eposta));
//                msg.Subject = "Hoşgeldiniz";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/Kurumsal-Registration.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{adsoyad}", adSoyad);
//                body = body.Replace("{sirketAdi}", SirketAdi);
//                body = body.Replace("{kurumkodu}", KurumKodu);
//                msg.Body = body;
//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                client.Send(msg);
//            }
//        }
//        public static void BireyselKullaniciHosgeldinMailiGonder(Guid userid)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                string adSoyad = Helper.AdSoyadGetir(userid.ToString());
//                string eposta = Membership.GetUser(userid).UserName;
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.To.Add(new MailAddress(eposta));
//                msg.Subject = "Hoşgeldiniz";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/Bireysel-Registration.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{adsoyad}", adSoyad);
//                msg.Body = body;
//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                client.Send(msg);
//            }
//        }
//        public static bool SiparisTamamlandiMailiGonder(int Orderid, string Konu)
//        {

//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                // msg.To.Add(new MailAddress("bykezza@gmail.com"));
//                msg.To.Add(new MailAddress(ord.aspnet_User.LoweredUserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/siparis-tamamlandi.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                string KurumAdi = "";
//                if (ord.KurumsalisMi == true)
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(ord.UserId.ToString()));
//                body = body.Replace("{Dosyaindirlinki}", "<a href=\"https://ceviridukkani.com/DosyaZiple.aspx?orderid=" + Orderid.ToString() + "&userid=" + ord.UserId.ToString() + "\">Dosyaların tümünü indir</a>");

//                body = body.Replace("{tabloYukle}", Helper.MusteriIcinKaynakveHedefDosyalarTablosuOlustur(ord));
//                string DokumanTipi;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                }
//                if (ord.YeminliMi != null)
//                {
//                    if (ord.YeminliMi == true)
//                    {
//                        body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                    }
//                    else
//                    {
//                        body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                    }
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }
//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("alınmıştır", "tamamlanmıştır. Aşağıdaki linke tıklayarak çevirinizi indirebilirsiniz.");
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));
//                string Karaktersayisi;
//                if (ord.KarakterSayisi != null | ord.KarakterSayisi != 0)
//                {
//                    Karaktersayisi = ord.KarakterSayisi.ToString();

//                }
//                else
//                {
//                    Karaktersayisi = "-";

//                }
//                List<Terminoloji_Order> toList = db.Terminoloji_Orders.Where(a => a.OrderId == Orderid).ToList();
//                if (toList.Count > 0)
//                {
//                    string Terminolojiler = "";
//                    foreach (Terminoloji_Order item in toList)
//                    {
//                        Terminolojiler += "<a href='http://ceviridukkani.com/" + item.Terminoloji.DosyaYolu + "'>" + item.Terminoloji.TerminolojiAdi + "</a><br>";
//                    }
//                    body = body.Replace("{TerminolojiDosyasi}", "<tr><td>Terminoloji/Referans:</td><td>" + Terminolojiler + "</td></tr>");

//                }
//                else
//                {
//                    body = body.Replace("{TerminolojiDosyasi}", "");
//                }
//                body = body.Replace("{KarakterSayisi}", Karaktersayisi);
//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());
//                body = body.Replace("{islemTarihi}", Convert.ToDateTime(ord.OrderTarihi).ToShortDateString() + " - " + Convert.ToDateTime(ord.OrderTarihi).ToShortTimeString());
//                string TeslimTarihi;
//                if (ord.TeslimTarihi != null)
//                {
//                    TeslimTarihi = Convert.ToDateTime(ord.TeslimTarihi).ToShortDateString();
//                }
//                else
//                {
//                    TeslimTarihi = "-";
//                }
//                body = body.Replace("{teslimTarihi}", TeslimTarihi);


//                body = body.Replace("{tutar}", String.Format("{0:#,###}", ord.Fiyat));
//                string tutarStringi = String.Format("{0:0.00}", Convert.ToDecimal(ord.Fiyat));
//                body = body.Replace("{tutar}", tutarStringi);
//                double kdv = (Convert.ToDouble(ord.Fiyat) * 18) / 100;
//                string kdvString = String.Format("{0:0.00}", Convert.ToDecimal(kdv));
//                body = body.Replace("{kdv}", kdvString);
//                double Toplam = kdv + Convert.ToDouble(ord.Fiyat);
//                string ToplamStringi = String.Format("{0:0.00}", Convert.ToDecimal(Toplam));
//                body = body.Replace("{ucret}", ToplamStringi);

//                body = body.Replace("{odemeSekli}", ord.OdemeSekli);
//                if (Convert.ToBoolean(ord.OdemeDurumu))
//                {
//                    body = body.Replace("{odemeYapildiMi}", "Yapıldı");
//                }
//                else
//                {
//                    body = body.Replace("{odemeYapildiMi}", "Yapılmadı");
//                }
//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);
//                if (ord.MusteriyeGonderilecekNot == null || ord.MusteriyeGonderilecekNot == "")
//                {
//                    body = body.Replace("{MusteriNotu}", "");
//                }
//                else
//                {
//                    body = body.Replace("{MusteriNotu}", "<div class=\"ekyazi\"><b>Size söylemek istediğimiz birşey var:</b><br>" + ord.MusteriyeGonderilecekNot + "</div>");
//                }



//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//        public static bool OdemeAlindiMailiGonder(int Orderid, string Konu)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.To.Add(new MailAddress(ord.aspnet_User.UserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/odeme-alindi.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                string KurumAdi = "";
//                if (ord.KurumsalisMi == true)
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(ord.UserId.ToString()));
//                if (ord.YeminliMi != null)
//                {
//                    if (ord.YeminliMi == true)
//                    {
//                        body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                    }
//                    else
//                    {
//                        body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                    }
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }
//                string DokumanTipi, Dokuman;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                    Dokuman = "";
//                    int i = 1;
//                    foreach (Dosya item in ord.Dosyas)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "'>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " karakter" + "</a><br/>";
//                        i++;
//                    }
//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                    Dokuman = ord.GirilenMetin;
//                }
//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));
//                string Karaktersayisi;
//                if (ord.KarakterSayisi != null | ord.KarakterSayisi != 0)
//                {
//                    Karaktersayisi = ord.KarakterSayisi.ToString();

//                }
//                else
//                {
//                    Karaktersayisi = "-";

//                }
//                if (ord.YeminliMi != null)
//                {
//                    if (ord.YeminliMi == true)
//                    {
//                        body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                    }
//                    else
//                    {
//                        body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                    }
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }
//                body = body.Replace("{KarakterSayisi}", Karaktersayisi);
//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());
//                body = body.Replace("{islemTarihi}", Convert.ToDateTime(ord.OrderTarihi).ToShortDateString() + " - " + Convert.ToDateTime(ord.OrderTarihi).ToShortTimeString());
//                string TeslimTarihi;
//                if (ord.TeslimTarihi != null)
//                {
//                    TeslimTarihi = Convert.ToDateTime(ord.TeslimTarihi).ToShortDateString();
//                }
//                else
//                {
//                    TeslimTarihi = "-";
//                }
//                body = body.Replace("{teslimTarihi}", TeslimTarihi);
//                string tutarStringi = String.Format("{0:0.00}", Convert.ToDecimal(ord.Fiyat));
//                body = body.Replace("{tutar}", tutarStringi);
//                double kdv = (Convert.ToDouble(ord.Fiyat) * 18) / 100;
//                string kdvString = String.Format("{0:0.00}", Convert.ToDecimal(kdv));
//                body = body.Replace("{kdv}", kdvString);
//                double Toplam = kdv + Convert.ToDouble(ord.Fiyat);
//                string ToplamStringi = String.Format("{0:0.00}", Convert.ToDecimal(Toplam));
//                body = body.Replace("{ucret}", ToplamStringi);
//                body = body.Replace("{odemeSekli}", ord.OdemeSekli);
//                if (Convert.ToBoolean(ord.OdemeDurumu))
//                {
//                    body = body.Replace("{odemeYapildiMi}", "Yapıldı");
//                }
//                else
//                {
//                    body = body.Replace("{odemeYapildiMi}", "Yapılmadı");
//                }


//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);
//                if (ord.MusteriyeGonderilecekNot == null || ord.MusteriyeGonderilecekNot == "")
//                {
//                    body = body.Replace("{MusteriNotu}", "");
//                }
//                else
//                {
//                    body = body.Replace("{MusteriNotu}", "<div class=\"ekyazi\"><b>Size söylemek istediğimiz birşey var:</b><br>" + ord.MusteriyeGonderilecekNot + "</div>");
//                }

//                List<Terminoloji_Order> toList = db.Terminoloji_Orders.Where(a => a.OrderId == Orderid).ToList();
//                if (toList.Count > 0)
//                {
//                    string Terminolojiler = "";
//                    foreach (Terminoloji_Order item in toList)
//                    {
//                        Terminolojiler += "<a href='http://ceviridukkani.com/" + item.Terminoloji.DosyaYolu + "'>" + item.Terminoloji.TerminolojiAdi + "</a><br>";
//                    }
//                    body = body.Replace("{TerminolojiDosyasi}", "<tr><td>Terminoloji/Referans:</td><td>" + Terminolojiler + "</td></tr>");

//                }
//                else
//                {
//                    body = body.Replace("{TerminolojiDosyasi}", "");
//                }

//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//        public static bool TeklifRedAciklamasiMailiGonder(int TeklifId, string Konu)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Teklif tkl = db.Teklifs.Where(a => a.TeklifId == TeklifId).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");

//                // msg.To.Add(new MailAddress("ugrprmk@hotmail.com"));
//                msg.To.Add(new MailAddress(tkl.aspnet_User.UserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/teklifredaciklamasi.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                string KurumAdi = "";
//                if (tkl.KurumsalisMi == true)
//                {
//                    KurumAdi = tkl.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{userid}", tkl.UserId.ToString());
//                body = body.Replace("{teklifAciklamasi}", tkl.MusteriyeGidecekTeklifAciklamasi);
//                body = body.Replace("{teklifRedAciklamasi}", tkl.TeklifRedSebebi);

//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(tkl.UserId.ToString()));
//                if (tkl.YeminliMi == true)
//                {
//                    body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }

//                string DokumanTipi, Dokuman;

//                DokumanTipi = "Dosya Yükleyici";
//                Dokuman = "";
//                int i = 1;
//                foreach (Dosya item in tkl.Dosyas)
//                {
//                    if (item.KarakterSayisi > 0)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "' download>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " Karakter" + "</a><br/>";
//                    }
//                    else
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "' download>" + i + " - " + item.ilkDosyaAdi + "</a><br/>";
//                    }

//                    i++;
//                }

//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(tkl.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(tkl.HedefDiller.ToString()));


//                body = body.Replace("{alan}", tkl.Alan.Adi);
//                body = body.Replace("{karakterSayisi}", tkl.KarakterSayisi.ToString());
//                double fiyat = Convert.ToDouble(tkl.Fiyat);
//                body = body.Replace("{fiyat}", String.Format("{0:0.00}", Convert.ToDecimal(fiyat)) + " TL");
//                double kdv = (fiyat * 18) / 100;
//                string kdvString = String.Format("{0:0.00}", Convert.ToDecimal(kdv));
//                body = body.Replace("{kdv}", kdvString);
//                double Toplam = kdv + fiyat;
//                string ToplamStringi = String.Format("{0:0.00}", Convert.ToDecimal(Toplam));
//                body = body.Replace("{ucret}", ToplamStringi);
//                body = body.Replace("{teklifId}", TeklifId.ToString());
//                body = body.Replace("{teslimTarihi}", Convert.ToDateTime(tkl.TeslimEdilecekTarih).ToShortDateString());
//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", tkl.TeklifNotu);
//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//        public static bool TeklifKabulEdilmediMailiGonder(int TeklifId, string Konu)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Teklif tkl = db.Teklifs.Where(a => a.TeklifId == TeklifId).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");

//                //msg.To.Add(new MailAddress("ugrprmk@hotmail.com"));
//                msg.To.Add(new MailAddress(tkl.aspnet_User.UserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/teklifrededildi.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                string KurumAdi = "";
//                if (tkl.KurumsalisMi == true)
//                {
//                    KurumAdi = tkl.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{userid}", tkl.UserId.ToString());
//                body = body.Replace("{teklifAciklamasi}", tkl.MusteriyeGidecekTeklifAciklamasi);
//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(tkl.UserId.ToString()));
//                if (tkl.YeminliMi == true)
//                {
//                    body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }

//                string DokumanTipi, Dokuman;

//                DokumanTipi = "Dosya Yükleyici";
//                Dokuman = "";
//                int i = 1;
//                foreach (Dosya item in tkl.Dosyas)
//                {
//                    if (item.KarakterSayisi > 0)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "' download>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " Karakter" + "</a><br/>";
//                    }
//                    else
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "' download>" + i + " - " + item.ilkDosyaAdi + "</a><br/>";
//                    }

//                    i++;
//                }
//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(tkl.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(tkl.HedefDiller.ToString()));


//                body = body.Replace("{alan}", tkl.Alan.Adi);
//                body = body.Replace("{karakterSayisi}", tkl.KarakterSayisi.ToString());
//                double fiyat = Convert.ToDouble(tkl.Fiyat);
//                body = body.Replace("{fiyat}", String.Format("{0:0.00}", Convert.ToDecimal(fiyat)) + " TL");
//                double kdv = (fiyat * 18) / 100;
//                string kdvString = String.Format("{0:0.00}", Convert.ToDecimal(kdv));
//                body = body.Replace("{kdv}", kdvString);
//                double Toplam = kdv + fiyat;
//                string ToplamStringi = String.Format("{0:0.00}", Convert.ToDecimal(Toplam));
//                body = body.Replace("{ucret}", ToplamStringi);
//                body = body.Replace("{teklifId}", TeklifId.ToString());
//                body = body.Replace("{teslimTarihi}", Convert.ToDateTime(tkl.TeslimEdilecekTarih).ToShortDateString());
//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", tkl.TeklifNotu);
//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//        public static bool TeklifCevabiMailiGonder(int TeklifId, string Konu)
//        {

//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Teklif tkl = db.Teklifs.Where(a => a.TeklifId == TeklifId).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");

//                //msg.To.Add(new MailAddress("ugrprmk@hotmail.com"));
//                msg.To.Add(new MailAddress(tkl.aspnet_User.UserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/teklifcevabi.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                string KurumAdi = "";
//                if (tkl.KurumsalisMi == true)
//                {
//                    KurumAdi = tkl.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{userid}", tkl.UserId.ToString());
//                body = body.Replace("{teklifAciklamasi}", tkl.MusteriyeGidecekTeklifAciklamasi);
//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(tkl.UserId.ToString()));
//                if (tkl.YeminliMi == true)
//                {
//                    body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }

//                string DokumanTipi, Dokuman;

//                DokumanTipi = "Dosya Yükleyici";
//                Dokuman = "";
//                int i = 1;
//                foreach (Dosya item in tkl.Dosyas)
//                {
//                    Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "' download>" + i + " - " + item.ilkDosyaAdi + "</a><br/>";
//                    i++;
//                }

//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(tkl.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(tkl.HedefDiller.ToString()));


//                body = body.Replace("{alan}", tkl.Alan.Adi);
//                body = body.Replace("{karakterSayisi}", tkl.KarakterSayisi.ToString());
//                double fiyat = Convert.ToDouble(tkl.Fiyat);
//                body = body.Replace("{fiyat}", String.Format("{0:0.00}", Convert.ToDecimal(fiyat)) + " TL");

//                double kdv = (fiyat * 18) / 100;
//                string kdvString = String.Format("{0:0.00}", Convert.ToDecimal(kdv));
//                body = body.Replace("{kdv}", kdvString);
//                double Toplam = kdv + fiyat;
//                string ToplamStringi = String.Format("{0:0.00}", Convert.ToDecimal(Toplam));
//                body = body.Replace("{ucret}", ToplamStringi);
//                body = body.Replace("{teklifId}", TeklifId.ToString());
//                body = body.Replace("{teslimTarihi}", Convert.ToDateTime(tkl.TeslimEdilecekTarih).ToShortDateString());
//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", tkl.TeklifNotu);
//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//        public static bool TeklifTalebiMailiGonder(int TeklifId, string Konu)
//        {

//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Teklif tkl = db.Teklifs.Where(a => a.TeklifId == TeklifId).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");

//                //msg.To.Add(new MailAddress("ugrprmk@hotmail.com"));
//                msg.To.Add(new MailAddress(tkl.aspnet_User.UserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/tekliftalebi.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                string KurumAdi = "";
//                if (tkl.KurumsalisMi == true)
//                {
//                    KurumAdi = tkl.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(tkl.UserId.ToString()));
//                if (tkl.YeminliMi == true)
//                {
//                    body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }

//                string DokumanTipi, Dokuman;

//                DokumanTipi = "Dosya Yükleyici";
//                Dokuman = "";
//                int i = 1;
//                foreach (Dosya item in tkl.Dosyas)
//                {
//                    if (item.KarakterSayisi > 0)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "' download>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " Karakter" + "</a><br/>";
//                    }
//                    else
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "' download>" + i + " - " + item.ilkDosyaAdi + "</a><br/>";
//                    }

//                    i++;
//                }

//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(tkl.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(tkl.HedefDiller.ToString()));


//                body = body.Replace("{alan}", tkl.Alan.Adi);
//                body = body.Replace("{teklifId}", TeklifId.ToString());
//                body = body.Replace("{islemTarihi}", Convert.ToDateTime(tkl.TeklifTarihi).ToShortDateString() + " - " + Convert.ToDateTime(tkl.TeklifTarihi).ToShortTimeString());
//                string TeslimTarihi;
//                if (tkl.istenilenTeslimTarihi != null)
//                {
//                    TeslimTarihi = Convert.ToDateTime(tkl.istenilenTeslimTarihi).ToShortDateString();
//                }
//                else
//                {
//                    TeslimTarihi = "-";
//                }
//                body = body.Replace("{teslimTarihi}", TeslimTarihi);



//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", tkl.TeklifNotu);



//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }

//        public static bool SiparisMailiGonder(int Orderid, string Konu)
//        {

//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);


//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");

//                //msg.To.Add(new MailAddress("ugrprmk@hotmail.com"));

//                msg.To.Add(new MailAddress(ord.aspnet_User.UserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/siparis.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                string KurumAdi = "";
//                if (ord.KurumsalisMi == true)
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(ord.UserId.ToString()));
//                if (ord.YeminliMi != null)
//                {
//                    if (ord.YeminliMi == true)
//                    {
//                        body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                    }
//                    else
//                    {
//                        body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                    }
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }
//                string DokumanTipi, Dokuman;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                    Dokuman = "";
//                    int i = 1;
//                    foreach (Dosya item in ord.Dosyas)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "'>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " karakter" + "</a><br/>";
//                        i++;
//                    }
//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                    Dokuman = ord.GirilenMetin;
//                }
//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));
//                string Karaktersayisi;
//                if (ord.KarakterSayisi != null | ord.KarakterSayisi != 0)
//                {
//                    Karaktersayisi = ord.KarakterSayisi.ToString();

//                }
//                else
//                {
//                    Karaktersayisi = "-";

//                }
//                if (ord.YeminliMi != null)
//                {
//                    if (ord.YeminliMi == true)
//                    {
//                        body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                    }
//                    else
//                    {
//                        body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                    }
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }
//                body = body.Replace("{KarakterSayisi}", Karaktersayisi);
//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());
//                body = body.Replace("{islemTarihi}", Convert.ToDateTime(ord.OrderTarihi).ToShortDateString() + " - " + Convert.ToDateTime(ord.OrderTarihi).ToShortTimeString());
//                string TeslimTarihi;
//                if (ord.TeslimTarihi != null)
//                {
//                    TeslimTarihi = Convert.ToDateTime(ord.TeslimTarihi).ToShortDateString();
//                }
//                else
//                {
//                    TeslimTarihi = "-";
//                }
//                body = body.Replace("{teslimTarihi}", TeslimTarihi);
//                string tutarStringi = String.Format("{0:0.00}", Convert.ToDecimal(ord.Fiyat));
//                body = body.Replace("{tutar}", tutarStringi);
//                double kdv = (Convert.ToDouble(ord.Fiyat) * 18) / 100;
//                string kdvString = String.Format("{0:0.00}", Convert.ToDecimal(kdv));
//                body = body.Replace("{kdv}", kdvString);
//                double Toplam = kdv + Convert.ToDouble(ord.Fiyat);
//                string ToplamStringi = String.Format("{0:0.00}", Convert.ToDecimal(Toplam));
//                body = body.Replace("{ucret}", ToplamStringi);
//                body = body.Replace("{odemeSekli}", ord.OdemeSekli);
//                if (Convert.ToBoolean(ord.OdemeDurumu))
//                {
//                    body = body.Replace("{odemeYapildiMi}", "Yapıldı");
//                }
//                else
//                {
//                    body = body.Replace("{odemeYapildiMi}", "Yapılmadı");
//                }


//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);
//                if (ord.MusteriyeGonderilecekNot == null || ord.MusteriyeGonderilecekNot == "")
//                {
//                    body = body.Replace("{MusteriNotu}", "");
//                }
//                else
//                {
//                    body = body.Replace("{MusteriNotu}", "<div class=\"ekyazi\"><b>Size söylemek istediğimiz birşey var:</b><br>" + ord.MusteriyeGonderilecekNot + "</div>");
//                }

//                List<Terminoloji_Order> toList = db.Terminoloji_Orders.Where(a => a.OrderId == Orderid).ToList();
//                if (toList.Count > 0)
//                {
//                    string Terminolojiler = "";
//                    foreach (Terminoloji_Order item in toList)
//                    {
//                        Terminolojiler += "<a href='http://ceviridukkani.com/" + item.Terminoloji.DosyaYolu + "'>" + item.Terminoloji.TerminolojiAdi + "</a><br>";
//                    }
//                    body = body.Replace("{TerminolojiDosyasi}", "<tr><td>Terminoloji/Referans:</td><td>" + Terminolojiler + "</td></tr>");

//                }
//                else
//                {
//                    body = body.Replace("{TerminolojiDosyasi}", "");
//                }

//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }

//        public static bool ihaleAcildiMailiGonder(int ihaleId, string AliciEposta)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                ihale ihl = db.ihales.Where(a => a.ihaleId == ihaleId).FirstOrDefault();
//                Order ord = ihl.Order;
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.To.Add(new MailAddress(AliciEposta));
//                msg.Subject = "JobNo:" + ord.OrderId.ToString() + " Çeviri Dükkanı, Yeni İş Teklifi!";
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/ihaleye-is-ac.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                if (AliciEposta.Contains("ceviridukkani.com"))
//                {
//                    body = body.Replace("{adsoyad}", "Yetkili");
//                }
//                else
//                {
//                    body = body.Replace("{adsoyad}", Helper.AdSoyadGetirbyEmail(AliciEposta));
//                }
//                string tutar = "Brüt: " + String.Format("{0:#,###}", ihl.ToplamTutar) + " " + ihl.ParaBirimi;
//                tutar += "<br/>Stopaj(%17): -" + String.Format("{0:#,###}", ihl.ToplamTutar - (ihl.ToplamTutar * (decimal)0.83)) + " " + ihl.ParaBirimi;
//                tutar += "<br>Net: " + String.Format("{0:#,###}", ihl.ToplamTutar * (decimal)0.83) + " " + ihl.ParaBirimi;
//                body = body.Replace("{tutar}", tutar);
//                body = body.Replace("{ihaleid}", ihaleId.ToString());
//                if (AliciEposta.Contains("ceviridukkani.com"))
//                {
//                    body = body.Replace("{tid}", "");

//                }
//                else
//                {
//                    body = body.Replace("{tid}", Helper.GetUserIdByEmail(AliciEposta));
//                }


//                string Dokuman;
//                Dokuman = ihl.OrnekMetin;
//                //if (ord.GirilenMetin == null || ord.GirilenMetin == "")
//                //{
//                //    Dokuman = "";
//                //    int i = 1;
//                //    foreach (Dosya item in ord.Dosyas)
//                //    {
//                //        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "'>" + i + " - " + item.ilkDosyaAdi + " - " + item.BosluksuzKarakterSayisi + " karakter" + "</a><br/>";
//                //        i++;
//                //    }
//                //}
//                //else
//                //{
//                //    Dokuman = ord.GirilenMetin;
//                //}
//                body = body.Replace("{KaynakDil}", Helper.DilGetir((int)ihl.KaynakDilId));
//                body = body.Replace("{HedefDil}", Helper.DilGetir((int)ihl.HedefDilId));
//                string Karaktersayisi;

//                Karaktersayisi = ihl.KarakterSayisi.ToString();

//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());
//                body = body.Replace("{cevirinumarasi}", ord.OrderId.ToString());
//                string TeslimTarihi;
//                if (ihl.TercumanTeslimTarihi != null)
//                {
//                    TeslimTarihi = Convert.ToDateTime(ihl.TercumanTeslimTarihi).ToShortDateString() + " - " + Convert.ToDateTime(ihl.TercumanTeslimTarihi).ToShortTimeString();
//                }
//                else
//                {
//                    TeslimTarihi = "-";
//                }
//                body = body.Replace("{teslimTarihi}", TeslimTarihi);
//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{karakterSayisi}", ihl.KarakterSayisi.ToString());

//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);
//                if (ord.BizimTercumanaNotumuz == null || ord.BizimTercumanaNotumuz == "")
//                {
//                    body = body.Replace("{TercumanOzelNot}", "Özel Not bulunmuyor, İyi çalışmalar Dileriz");
//                }
//                else
//                {
//                    body = body.Replace("{TercumanOzelNot}", ord.BizimTercumanaNotumuz);
//                }
//                if (ord.YeminliMi != null)
//                {
//                    if (ord.YeminliMi == true)
//                    {
//                        body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                    }
//                    else
//                    {
//                        body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                    }
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }
//                List<Terminoloji_Order> toList = db.Terminoloji_Orders.Where(a => a.OrderId == ord.OrderId).ToList();
//                if (toList.Count > 0)
//                {
//                    string Terminolojiler = "";
//                    foreach (Terminoloji_Order item in toList)
//                    {
//                        Terminolojiler += "<a href='http://ceviridukkani.com/" + item.Terminoloji.DosyaYolu + "'>" + item.Terminoloji.TerminolojiAdi + "</a><br>";
//                    }
//                    body = body.Replace("{TerminolojiDosyasi}", "<tr><td>Terminoloji/Referans:</td><td>" + Terminolojiler + "</td></tr>");

//                }
//                else
//                {
//                    body = body.Replace("{TerminolojiDosyasi}", "");
//                }



//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }



//        public static bool TercumanaBilgiMailiGonder(int Orderid, string Konu, string TercumanEposta, string tercumanAdSoyad, string HedefDil)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.To.Add(new MailAddress(TercumanEposta));
//                //msg.To.Add(new MailAddress("ugrprmk@hotmail.com"));
//                msg.Subject = Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/tercuman-is-gonder.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);

//                body = body.Replace("{adsoyad}", tercumanAdSoyad);
//                string Dokuman;
//                if (ord.GirilenMetin == null || ord.GirilenMetin == "")
//                {
//                    Dokuman = "";
//                    int i = 1;
//                    foreach (Dosya item in ord.Dosyas)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "'>" + i + " - " + item.ilkDosyaAdi + " - " + item.BosluksuzKarakterSayisi + " karakter" + "</a><br/>";
//                        i++;
//                    }
//                }
//                else
//                {
//                    Dokuman = ord.GirilenMetin;
//                }
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", HedefDil);
//                string Karaktersayisi;
//                if (ord.BosluksuzKarakterSayisi != null | ord.BosluksuzKarakterSayisi != 0)
//                {
//                    Karaktersayisi = ord.BosluksuzKarakterSayisi.ToString();

//                }
//                else
//                {
//                    Karaktersayisi = "-";
//                }
//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());
//                body = body.Replace("{cevirinumarasi}", ord.OrderId.ToString());
//                if (Roles.IsUserInRole(TercumanEposta, "Freelance"))
//                {
//                    decimal tercumanaOdenecekMiktar = (decimal)db.Order_Tercumans.Where(a => a.Dosya.OrderId == Orderid && a.aspnet_User.LoweredUserName == TercumanEposta.ToLower()).FirstOrDefault().TercumanaOdenecekMiktar;
//                    string tutar = "Brüt: " + String.Format("{0:#,###}", tercumanaOdenecekMiktar) + " TL";
//                    tutar += "<br/>Stopaj(%17): -" + String.Format("{0:#,###}", tercumanaOdenecekMiktar - (tercumanaOdenecekMiktar * (decimal)0.83)) + " TL";
//                    tutar += "<br>Net: " + String.Format("{0:#,###}", tercumanaOdenecekMiktar * (decimal)0.83) + " TL";
//                    body = body.Replace("{tercumanUcreti}", "<tr><td valign=\"top\">Ödenecek Tutar:</td><td>" + tutar + "</td></tr>");
//                }
//                else
//                {
//                    body = body.Replace("{tercumanUcreti}", "");
//                }
//                string TeslimTarihi;
//                if (ord.TercumanTeslimTarihi != null)
//                {
//                    TeslimTarihi = Convert.ToDateTime(ord.TercumanTeslimTarihi).ToShortDateString() + " - " + Convert.ToDateTime(ord.TercumanTeslimTarihi).ToShortTimeString();
//                }
//                else
//                {
//                    TeslimTarihi = "-";
//                }
//                body = body.Replace("{teslimTarihi}", TeslimTarihi);
//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);
//                if (ord.BizimTercumanaNotumuz == null || ord.BizimTercumanaNotumuz == "")
//                {
//                    body = body.Replace("{TercumanOzelNot}", "Özel Not bulunmuyor, İyi çalışmalar Dileriz");
//                }
//                else
//                {
//                    body = body.Replace("{TercumanOzelNot}", ord.BizimTercumanaNotumuz);
//                }
//                if (ord.YeminliMi != null)
//                {
//                    if (ord.YeminliMi == true)
//                    {
//                        body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                    }
//                    else
//                    {
//                        body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                    }
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }
//                List<Terminoloji_Order> toList = db.Terminoloji_Orders.Where(a => a.OrderId == Orderid).ToList();
//                if (toList.Count > 0)
//                {
//                    string Terminolojiler = "";
//                    foreach (Terminoloji_Order item in toList)
//                    {
//                        Terminolojiler += "<a href='http://ceviridukkani.com/" + item.Terminoloji.DosyaYolu + "'>" + item.Terminoloji.TerminolojiAdi + "</a><br>";
//                    }
//                    body = body.Replace("{TerminolojiDosyasi}", "<tr><td>Terminoloji/Referans:</td><td>" + Terminolojiler + "</td></tr>");

//                }
//                else
//                {
//                    body = body.Replace("{TerminolojiDosyasi}", "");
//                }



//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }

//        public static bool InsanKaynaklariMailiGonder(int Orderid, string Konu)
//        {

//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);


//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.To.Add(new MailAddress("hr@aim-tr.com"));
//                msg.Subject = "JobNo:" + Orderid + " " + Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/insan-kaynaklari.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                string KurumAdi = "";
//                if (ord.KurumsalisMi == true)
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                else
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Ad + " " + ord.aspnet_User.BireyselProfils[0].Soyad;
//                }
//                body = body.Replace("{adsoyad}", KurumAdi);
//                string DokumanTipi, Dokuman;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                    Dokuman = "";
//                    int i = 1;
//                    foreach (Dosya item in ord.Dosyas)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "'>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " karakter" + "</a><br/>";
//                        i++;
//                    }
//                    //Dokuman = "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/" + ord.DosyaYolu + "'>İNDİR</a>";

//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                    Dokuman = ord.GirilenMetin;
//                }
//                //body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));
//                if (ord.YeminliMi != null)
//                {
//                    if (ord.YeminliMi == true)
//                    {
//                        body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                    }
//                    else
//                    {
//                        body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                    }
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }

//                string Karaktersayisi;
//                if (ord.KarakterSayisi != null | ord.KarakterSayisi != 0)
//                {
//                    Karaktersayisi = ord.KarakterSayisi.ToString();

//                }
//                else
//                {
//                    Karaktersayisi = "-";

//                }
//                body = body.Replace("{KarakterSayisi}", Karaktersayisi);
//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());
//                body = body.Replace("{islemTarihi}", DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString());
//                string TeslimTarihi;
//                if (ord.TeslimTarihi != null)
//                {
//                    TeslimTarihi = Convert.ToDateTime(ord.TeslimTarihi).ToShortDateString();
//                }
//                else
//                {
//                    TeslimTarihi = "-";
//                }
//                body = body.Replace("{teslimTarihi}", TeslimTarihi);
//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);

//                List<Terminoloji_Order> toList = db.Terminoloji_Orders.Where(a => a.OrderId == Orderid).ToList();
//                if (toList.Count > 0)
//                {
//                    string Terminolojiler = "";
//                    foreach (Terminoloji_Order item in toList)
//                    {
//                        Terminolojiler += "<a href='http://ceviridukkani.com/" + item.Terminoloji.DosyaYolu + "'>" + item.Terminoloji.TerminolojiAdi + "</a><br>";
//                    }
//                    body = body.Replace("{TerminolojiDosyasi}", "<tr><td>Terminoloji/Referans:</td><td>" + Terminolojiler + "</td></tr>");

//                }
//                else
//                {
//                    body = body.Replace("{TerminolojiDosyasi}", "");
//                }
//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//        public static bool SiparisTamamlandiforIK(int Orderid, string Konu)
//        {

//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);

//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                //msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.To.Add(new MailAddress("hr@aim-tr.com"));
//                //msg.To.Add(new MailAddress("ugrprmk@hotmail.com"));

//                msg.Subject = "JobNo:" + Orderid + " " + Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/insan-kaynaklari.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);
//                string KurumAdi = "";
//                if (ord.KurumsalisMi == true)
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                else
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Ad + " " + ord.aspnet_User.BireyselProfils[0].Soyad;
//                }
//                body = body.Replace("{adsoyad}", KurumAdi);
//                string DokumanTipi, Dokuman;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                    Dokuman = "";
//                    int i = 1;
//                    foreach (Dosya item in ord.Dosyas)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "'>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " karakter" + "</a><br/>";
//                        i++;
//                    }
//                    //Dokuman = "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/" + ord.DosyaYolu + "'>İNDİR</a>";

//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                    Dokuman = ord.GirilenMetin;
//                }
//                if (ord.YeminliMi != null)
//                {
//                    if (ord.YeminliMi == true)
//                    {
//                        body = body.Replace("{yeminliTercume}", "Evet, Yeminli");
//                    }
//                    else
//                    {
//                        body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                    }
//                }
//                else
//                {
//                    body = body.Replace("{yeminliTercume}", "Hayır, değil");
//                }
//                //body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));
//                string Karaktersayisi;
//                if (ord.KarakterSayisi != null | ord.KarakterSayisi != 0)
//                {
//                    Karaktersayisi = ord.KarakterSayisi.ToString();

//                }
//                else
//                {
//                    Karaktersayisi = "-";

//                }
//                body = body.Replace("{KarakterSayisi}", Karaktersayisi);
//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());
//                body = body.Replace("{islemTarihi}", Convert.ToDateTime(ord.OrderTarihi).ToShortDateString() + " - " + Convert.ToDateTime(ord.OrderTarihi).ToShortTimeString());
//                string TeslimTarihi;
//                if (ord.TeslimTarihi != null)
//                {
//                    TeslimTarihi = Convert.ToDateTime(ord.TeslimTarihi).ToShortDateString();
//                }
//                else
//                {
//                    TeslimTarihi = "-";
//                }
//                List<Terminoloji_Order> toList = db.Terminoloji_Orders.Where(a => a.OrderId == Orderid).ToList();
//                if (toList.Count > 0)
//                {
//                    string Terminolojiler = "";
//                    foreach (Terminoloji_Order item in toList)
//                    {
//                        Terminolojiler += "<a href='http://ceviridukkani.com/" + item.Terminoloji.DosyaYolu + "'>" + item.Terminoloji.TerminolojiAdi + "</a><br>";
//                    }
//                    body = body.Replace("{TerminolojiDosyasi}", "<tr><td>Terminoloji/Referans:</td><td>" + Terminolojiler + "</td></tr>");

//                }
//                else
//                {
//                    body = body.Replace("{TerminolojiDosyasi}", "");
//                }
//                body = body.Replace("{teslimTarihi}", TeslimTarihi);
//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);

//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//        public static void TercumanSiparisiptalMailiGonder(int Orderid, string Konu, Guid tercumanid)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.To.Add(new MailAddress(Membership.GetUser(tercumanid).UserName));
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.Subject = "JobNo:" + Orderid + " " + Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/tercuman-siparis-iptal.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);

//                string DokumanTipi, Dokuman;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                    Dokuman = "";
//                    int i = 1;
//                    foreach (Dosya item in ord.Dosyas)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "'>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " karakter" + "</a><br/>";
//                        i++;
//                    }

//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                    Dokuman = ord.GirilenMetin;
//                }
//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));
//                string Karaktersayisi;
//                if (ord.KarakterSayisi != null | ord.KarakterSayisi != 0)
//                {
//                    Karaktersayisi = ord.KarakterSayisi.ToString();

//                }
//                else
//                {
//                    Karaktersayisi = "-";

//                }

//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());

//                body = body.Replace("{iptalTarihi}", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);

//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                client.Send(msg);
//            }
//        }
//        public static void TercumanSiparisiptalMailiGonder(int Orderid, string Konu)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                List<Order_Tercuman> tercumanlar = db.Order_Tercumans.Where(a => a.Dosya.OrderId == Orderid).ToList();
//                if (tercumanlar != null)
//                {
//                    foreach (Order_Tercuman item in tercumanlar)
//                    {
//                        msg.To.Add(new MailAddress(item.aspnet_User.UserName));
//                    }
//                }

//                msg.Subject = "JobNo:" + Orderid + " " + Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/tercuman-siparis-iptal.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);

//                string DokumanTipi, Dokuman;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                    Dokuman = "";
//                    int i = 1;
//                    foreach (Dosya item in ord.Dosyas)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "'>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " karakter" + "</a><br/>";
//                        i++;
//                    }

//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                    Dokuman = ord.GirilenMetin;
//                }
//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));
//                string Karaktersayisi;
//                if (ord.KarakterSayisi != null | ord.KarakterSayisi != 0)
//                {
//                    Karaktersayisi = ord.KarakterSayisi.ToString();

//                }
//                else
//                {
//                    Karaktersayisi = "-";

//                }

//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());

//                body = body.Replace("{iptalTarihi}", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);

//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                client.Send(msg);
//            }
//        }
//        public static void SiparisiptalMailiGonder(int Orderid, string Konu)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();
//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);
//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.To.Add(new MailAddress(ord.aspnet_User.UserName));
//                msg.Subject = "JobNo:" + Orderid + " " + Konu;

//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/siparis-iptal.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);

//                string DokumanTipi, Dokuman;
//                if (ord.GirilenMetin == null)
//                {
//                    DokumanTipi = "Dosya Yükleyici";
//                    Dokuman = "";
//                    int i = 1;
//                    foreach (Dosya item in ord.Dosyas)
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/UploadedFiles/" + item.DosyaAdi + "'>" + i + " - " + item.ilkDosyaAdi + " - " + item.KarakterSayisi + " karakter" + "</a><br/>";
//                        i++;
//                    }

//                }
//                else
//                {
//                    DokumanTipi = "Metin Girişi";
//                    Dokuman = ord.GirilenMetin;
//                }

//                body = body.Replace("{iptalTarihi}", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

//                body = body.Replace("{ceviritipi}", DokumanTipi);
//                body = body.Replace("{KaynakDil}", Helper.DilGetir(int.Parse(ord.KaynakDilId.ToString())));
//                body = body.Replace("{HedefDil}", Helper.HedefDilleriGetir(ord.HedefDiller.ToString()));
//                string Karaktersayisi;
//                if (ord.KarakterSayisi != null | ord.KarakterSayisi != 0)
//                {
//                    Karaktersayisi = ord.KarakterSayisi.ToString();

//                }
//                else
//                {
//                    Karaktersayisi = "-";

//                }
//                body = body.Replace("{KarakterSayisi}", Karaktersayisi);
//                body = body.Replace("{alan}", ord.Alan.Adi);
//                body = body.Replace("{ceviriId}", ord.OrderId.ToString());
//                body = body.Replace("{islemTarihi}", DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString());
//                string TeslimTarihi;
//                if (ord.TeslimTarihi != null)
//                {
//                    TeslimTarihi = Convert.ToDateTime(ord.TeslimTarihi).ToShortDateString();
//                }
//                else
//                {
//                    TeslimTarihi = "-";
//                }
//                body = body.Replace("{teslimTarihi}", TeslimTarihi);

//                body = body.Replace("{tutar}", String.Format("{0:#,###}", ord.Fiyat));
//                double kdv = (Convert.ToDouble(ord.Fiyat) * 18) / 100;
//                string kdvString = kdv.ToString();
//                body = body.Replace("{kdv}", kdvString);
//                double Toplam = kdv + Convert.ToDouble(ord.Fiyat);
//                string ToplamStringi = Toplam.ToString();
//                body = body.Replace("{ucret}", ToplamStringi);
//                body = body.Replace("{odemeSekli}", ord.OdemeSekli);
//                if (Convert.ToBoolean(ord.OdemeDurumu))
//                {
//                    body = body.Replace("{odemeYapildiMi}", "Yapıldı");
//                }
//                else
//                {
//                    body = body.Replace("{odemeYapildiMi}", "Yapılmadı");
//                }


//                body = body.Replace("{dokuman}", Dokuman);
//                body = body.Replace("{siparisNotu}", ord.TercumanNotu);

//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");

//                client.Send(msg);
//            }
//        }

//        public static bool CeviriGonderildiMailGonder(int Orderid, string Konu)
//        {
//            using (ceviridukkaniDataContext db = new ceviridukkaniDataContext())
//            {
//                bool gonderildiMi = false;
//                Order ord = db.Orders.Where(a => a.OrderId == Orderid).FirstOrDefault();

//                SmtpClient client = new SmtpClient("mail.ceviridukkani.com", 587);


//                MailMessage msg = new MailMessage();
//                msg.From = new MailAddress("noreply@ceviridukkani.com");
//                msg.Bcc.Add(new MailAddress("order@ceviridukkani.com"));
//                msg.To.Add(new MailAddress(ord.aspnet_User.UserName));
//                msg.Subject = "JobNo:" + Orderid + " " + Konu;
//                string body = string.Empty;
//                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/MailKaliplari/ceviri-tamamlandi.html")))
//                {
//                    body = reader.ReadToEnd();
//                }
//                body = body.Replace("{url}", "http://" + HttpContext.Current.Request.Url.Authority);

//                string KurumAdi = "";
//                if (ord.KurumsalisMi == true)
//                {
//                    KurumAdi = ord.aspnet_User.BireyselProfils[0].Kurum.SirketAdi + " - Gönderen: ";
//                }
//                body = body.Replace("{adsoyad}", KurumAdi + Helper.AdSoyadGetir(ord.UserId.ToString()));
//                string Dokuman = "";
//                if (ord.GirilenMetin == null)
//                {
//                    int i = 1;
//                    foreach (CevrilmisDosya item in ord.CevrilmisDosyas.OrderBy(a => a.DosyaAdi))
//                    {
//                        Dokuman += "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/CevrilmisDosyalar/" + item.DosyaAdi + "'>" + i + " - " + item.DosyaAdi + "</a><br/>";
//                        i++;
//                    }
//                    //Dokuman = "<a href='http://" + HttpContext.Current.Request.Url.Authority + "/" + ord.CevrilenDokuman + "'>İNDİR</a>";
//                }
//                //else
//                //{
//                //    Dokuman = ord.CevrilenMetin;
//                //}

//                body = body.Replace("{ceviri}", Dokuman);

//                List<Terminoloji_Order> toList = db.Terminoloji_Orders.Where(a => a.OrderId == Orderid).ToList();
//                if (toList.Count > 0)
//                {
//                    string Terminolojiler = "";
//                    foreach (Terminoloji_Order item in toList)
//                    {
//                        Terminolojiler += "<a href='http://ceviridukkani.com/" + item.Terminoloji.DosyaYolu + "'>" + item.Terminoloji.TerminolojiAdi + "</a><br>";
//                    }
//                    body = body.Replace("{TerminolojiDosyasi}", "<tr><td>Terminoloji/Referans:</td><td>" + Terminolojiler + "</td></tr>");

//                }
//                else
//                {
//                    body = body.Replace("{TerminolojiDosyasi}", "");
//                }

//                msg.Body = body;
//                msg.Priority = MailPriority.High;

//                msg.IsBodyHtml = true;
//                client.Credentials = new System.Net.NetworkCredential("noreply@ceviridukkani.com", "ceviridukkani");
//                try
//                {
//                    client.Send(msg);
//                    gonderildiMi = true;
//                }
//                catch (Exception)
//                {
//                    gonderildiMi = false;
//                }
//                return gonderildiMi;

//            }
//        }
//    }
//}
