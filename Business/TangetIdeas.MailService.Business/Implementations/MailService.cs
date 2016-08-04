using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.Domain.Entities.System;
using Tangent.CeviriDukkani.Domain.Exceptions;
using Tangent.CeviriDukkani.Domain.Exceptions.ExceptionCodes;
using TangetIdeas.MailService.Business.Interfaces;

namespace TangetIdeas.MailService.Business.Implementations
{
    public class MailService : IMailService
    {
        //internal ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CeviriDukkaniModel _model;
        private readonly IMailSenderService _mailSenderService;
        private readonly ILog _logger;

        public MailService(CeviriDukkaniModel model, IMailSenderService mailSenderService, ILog logger)
        {
            _model = model;
            _mailSenderService = mailSenderService;
            _logger = logger;
        }

        public ServiceResult GetWaitingMail()
        {
            var serviceResult = new ServiceResult();
            try
            {
                var mail = _model.Mail.Select(m => m.Status == MailStatusTypeEnum.Waiting);
                if (mail == null)
                {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = mail;
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                //Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult Add(MailItem mail)
        {
            var serviceResult = new ServiceResult(ServiceResultType.NotKnown);
            try
            {
                _model.Mail.Add(mail);

                serviceResult.Data = _model.SaveChanges() > 0;
                serviceResult.ServiceResultType = ServiceResultType.Success;
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult SendWaitingMails()
        {
            var serviceResult = new ServiceResult();
            try
            {
                var mails = _model.Mail.Include(a => a.To).Where(m => m.Status == MailStatusTypeEnum.Waiting).ToList();
                if (mails == null)
                {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }

                //Task.Run(() =>
                //{
                Parallel.ForEach(mails, i => { SendMail(i); });
                //});

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = mails;
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult SendMail(MailItem item)
        {
            var serviceResult = new ServiceResult();
            try
            {

                serviceResult = _mailSenderService.SendMail(item.Subject, item.Message, item.To, item.MailSender);
                if (serviceResult.ServiceResultType == ServiceResultType.Success)
                {
                    item.Status = MailStatusTypeEnum.Sent;
                    item.SendTime = DateTime.Now;
                }
                else
                {
                    item.Status = MailStatusTypeEnum.Error;
                    item.Exception = serviceResult.Exception.Message;
                }
                item.UpdatedAt = DateTime.Now;
                _model.Entry(item).State = EntityState.Modified;
                _model.SaveChanges();
                serviceResult.ServiceResultType = ServiceResultType.Success;
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult Add(SendMailRequestDto sendMailRequest)
        {
            var serviceResult = new ServiceResult(ServiceResultType.NotKnown);
            try
            {
                MailItem mailItem = new MailItem();

                mailItem.MailSender = sendMailRequest.MailSender;
                mailItem.Message = sendMailRequest.Message;
                mailItem.Subject = sendMailRequest.Subject;
                List<MailTarget> To = new List<MailTarget>();
                foreach (var item in sendMailRequest.To)
                {
                    To.Add(new MailTarget { MailAddres = item, Active = true });
                }
                mailItem.To = To;
                mailItem.Status = MailStatusTypeEnum.Progress;
                serviceResult = Add(mailItem);

                _mailSenderService.SendMail(mailItem);

            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        private MailSenderTypeEnum GetMailSender(MailType mailType)
        {
            MailSenderTypeEnum result = MailSenderTypeEnum.System;
            switch (mailType)
            {
                case MailType.Register:
                    result = MailSenderTypeEnum.User;
                    break;
                case MailType.ForgetPassword:
                    result = MailSenderTypeEnum.User;
                    break;
                case MailType.ResetPassword:
                    result = MailSenderTypeEnum.User;
                    break;
                case MailType.UserActivation:
                    result = MailSenderTypeEnum.User;
                    break;
                case MailType.Welcome:
                    result = MailSenderTypeEnum.User;
                    break;
                default:
                    result = MailSenderTypeEnum.System;
                    break;
            }

            return result;
        }

        private string PrepareMailMessage(MailType mailType, object data)
        {
            StringBuilder mailContent = new StringBuilder();

            mailContent.Append(File.ReadAllText("\\MailTemplates\\" + mailType.ToString() + "Template.html"));

            switch (mailType)
            {
                case MailType.Register:
                    var mailRegister = (MailData.Register)data;
                    mailContent.Replace("[USER-NAME]", mailRegister.UserName).Replace("[USER-EMAIL]", mailRegister.EMail).Replace("[USER-PASS]", mailRegister.Pass);
                    break;
                case MailType.ForgetPassword:
                    var mailDataForget = (MailData.ForgetPassword)data;
                    mailContent.Replace("[USER-NAME]", mailDataForget.UserName).Replace("[USER-EMAIL]", mailDataForget.EMail).Replace("[USER-PASS]", mailDataForget.Pass);
                    break;
                case MailType.ResetPassword:
                    var mailDataForget = (MailData.ForgetPassword)data;
                    mailContent.Replace("[USER-NAME]", mailDataForget.UserName).Replace("[RESET-LINK]", mailDataForget.EMail).Replace("[USER-PASS]", mailDataForget.Pass);
                    break;
                    break;
                case MailType.UserActivation:
                    var mailDataActivation = (MailData.ForgetPassword)data;

                    var memberCipher = StringCipher.Encrypt(userId.ToString());
                    mailContent = mailContent.Replace("[COMPANY-NAME]", "").Replace("[DETAIL]", " İsteğiniz tarafımıza ulaştı. Giriş yapabilmeniz için onaylamanız gerekmektedir.")
                        .Replace("[REGISTER-LINK]", $"http://kido-fun.com/Home/ActivateUser/{memberCipher}");
                    break;
                case MailType.Welcome:
                    break;
                default:
                    break;
            }
            StringBuilder emailFile = new StringBuilder();
            emailFile.Append(File.ReadAllText("\\MailTemplates\\EmailTemplate.html"));
            emailFile.Replace("[MAIL-TEMPLATE]", mailContent.ToString());
            return emailFile.ToString();
        }

        public static class StringCipher
        {
            // This constant is used to determine the keysize of the encryption algorithm in bits.
            // We divide this by 8 within the code below to get the equivalent number of bytes.
            private const int Keysize = 256;
            private const string KeyPhrase = "Pass1234";

            // This constant determines the number of iterations for the password bytes generation function.
            private const int DerivationIterations = 1000;

            public static string Encrypt(string plainText, string passPhrase = KeyPhrase)
            {
                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                // so that the same Salt and IV values can be used when decrypting.  
                var saltStringBytes = Generate256BitsOfRandomEntropy();
                var ivStringBytes = Generate256BitsOfRandomEntropy();
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;
                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                    var cipherTextBytes = saltStringBytes;
                                    cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                    cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                    memoryStream.Close();
                                    cryptoStream.Close();
                                    return Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                }
            }

            public static string Decrypt(string cipherText, string passPhrase = KeyPhrase)
            {
                // Get the complete stream of bytes that represent:
                // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
                var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
                // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
                var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
                // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
                var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
                // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
                var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

                using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = CipherMode.CBC;
                        symmetricKey.Padding = PaddingMode.PKCS7;
                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[cipherTextBytes.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    memoryStream.Close();
                                    cryptoStream.Close();
                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }

            private static byte[] Generate256BitsOfRandomEntropy()
            {
                var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    // Fill the array with cryptographically secure random bytes.
                    rngCsp.GetBytes(randomBytes);
                }
                return randomBytes;
            }

            public class MailData
            {

                public class Register
                {
                    public string UserName { get; set; }
                    public string EMail { get; set; }
                    public string Pass { get; set; }
                }

                public class ResetPassword
                {
                    public string UserName { get; set; }
                    public string ResetLink { get; set; }

                }

                public class ForgetPassword
                {
                    public string UserName { get; set; }
                    public string EMail { get; set; }
                    public string Pass { get; set; }
                }

                public class UserActivation
                {
                    public int UserId { get; set; }
                }
            }


        }
    }
}
