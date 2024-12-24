using ApiMETA.Models.Connections;
using System;
using System.Data;
using System.Net.Mail;
using System.Text;

namespace ApiMETA.Models.Classes
{
    public class EmailSender : IDisposable
    {
        public bool SendMail(DataTable dtConfiguration, string dtRemitente, string asunto, string cuerpo, string User)
        {
            // Inicializa la variable de respuesta como falsa.
            bool resp = false;
            // Variables para almacenar la dirección de correo y la clave de correo.
            string Mail = "";
            string claveMail = "";
            try
            {
                // Itera a través de las filas de la tabla de configuración para obtener la dirección y clave de correo.
                foreach (DataRow row in dtConfiguration.Rows)
                {
                    switch (row["Codigo"].ToString())
                    {
                        // Caso "Correo": obtiene la dirección y clave de correo.
                        case "Correo":
                            Mail = row["Valor"].ToString();
                            claveMail = row["Desencriptado"].ToString();
                            break;
                    }
                }
                // Crea un objeto MailMessage para construir el correo electrónico.
                MailMessage message = new MailMessage();
                // Establece el destinatario del correo.
                message.To.Add(dtRemitente);
                // Establece el asunto del correo.
                message.Subject = asunto;
                // Establece la dirección y nombre del remitente del correo.
                message.From = new MailAddress(Mail, "MOA - Serfinsa", Encoding.UTF8);
                // Configura la codificación del asunto del correo.
                message.SubjectEncoding = Encoding.UTF8;
                // Indica que el cuerpo del correo está en formato HTML.
                message.IsBodyHtml = true;
                // Configura la codificación del cuerpo del correo.
                message.BodyEncoding = Encoding.UTF8;
                // Establece el cuerpo del correo.
                message.Body = cuerpo;
                // Configuración del cliente SMTP para enviar el correo.
                SmtpClient SmtpMail = new SmtpClient();
                SmtpMail.Host = "smtp.gmail.com";
                SmtpMail.Port = 587;
                // Configura las credenciales del cliente SMTP.
                SmtpMail.Credentials = new System.Net.NetworkCredential(Mail, claveMail);
                // Configura el método de entrega del correo como Network (a través de la red).
                SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                // Habilita el uso de SSL para la conexión SMTP.
                SmtpMail.EnableSsl = true;
                // Configuración adicional para el cliente SMTP.
                SmtpMail.ServicePoint.MaxIdleTime = 0;
                SmtpMail.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);
                // Configura la codificación del cuerpo del correo.
                message.BodyEncoding = Encoding.Default;
                // Establece la prioridad del correo como alta.
                message.Priority = MailPriority.High;
                // Envía el correo electrónico.
                SmtpMail.Send(message);
                // Si el envío es exitoso, actualiza la variable de respuesta a verdadero.
                resp = true;
            }
            catch (Exception ex)
            {
                using (SqlProcess objSql = new SqlProcess())
                {
                    objSql.SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, User, Globals.IdSistema);
                }
            }
            return resp;
        }

        public bool SendVoucher(DataTable dtConfiguration, string dtRemitente, string asunto, string cuerpo, string voucher, string User)
        {
            bool resp = false;
            string Mail = "";
            string claveMail = "";
            try
            {
                foreach (DataRow row in dtConfiguration.Rows)
                {
                    switch (row["Codigo"].ToString())
                    {
                        case "Correo":
                            Mail = row["Valor"].ToString();
                            claveMail = row["Desencriptado"].ToString();
                            break;
                    }
                }

                MailMessage message = new MailMessage();
                message.To.Add(dtRemitente);
                message.Subject = asunto;
                message.From = new MailAddress(Mail, "MOA - Serfinsa", Encoding.UTF8);
                message.SubjectEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;
                message.Body = cuerpo;

                // Ruta completa del archivo adjunto (voucher en este caso).
                string path = Globals.PDFPath + voucher;

                // Crea un objeto Attachment para adjuntar el archivo al correo.
                Attachment attachment;
                attachment = new Attachment(path);
                // Agrega el archivo adjunto al mensaje de correo.
                message.Attachments.Add(attachment);

                SmtpClient SmtpMail = new SmtpClient();
                SmtpMail.Host = "smtp.gmail.com";
                SmtpMail.Port = 587;
                SmtpMail.Credentials = new System.Net.NetworkCredential(Mail, claveMail);
                SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpMail.EnableSsl = true;
                SmtpMail.ServicePoint.MaxIdleTime = 0;
                SmtpMail.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);
                message.BodyEncoding = Encoding.Default;
                message.Priority = MailPriority.High;
                // Envía el correo electrónico.
                SmtpMail.Send(message);
                // Si el envío es exitoso, actualiza la variable de respuesta a verdadero.
                resp = true;
            }
            catch (Exception ex)
            {
                using (SqlProcess objSql = new SqlProcess())
                {
                    objSql.SaveExceptions(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, User, Globals.IdSistema);
                }
            }
            return resp;
        }

        #region Disposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~SqlProcess()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal bool SendMail(object dtConfiguracion, string email, object asunto, object cuerpo, string user)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}