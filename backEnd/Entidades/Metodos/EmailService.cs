using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class EmailService
    {
        public async Task<int> EnviarEmailAsync(string destinatario)
        {
            // Generar el código de verificación
            Random random = new Random();
            int numeroVerificar = random.Next(1000, 9999);

            string bodyHtml = $@"
                <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                            <h2 style='color: #4CAF50; text-align: center;'>NutriApp - Verificación de Cuenta</h2>
                            <p>Hola,</p>
                            <p>Gracias por registrarte en NutriApp. Para completar tu registro, usa el siguiente código de verificación:</p>
                            <div style='text-align: center; margin: 20px;'>
                                <span style='font-size: 24px; font-weight: bold; color: #4CAF50;'>{numeroVerificar}</span>
                            </div>
                            <p>Este código es válido solo por un tiempo limitado. Si no has solicitado este código, ignora este mensaje.</p>
                            <p>¡Gracias por elegir NutriApp!</p>
                            <p style='font-size: 12px; color: #888;'>Este es un correo generado automáticamente, por favor, no respondas a este mensaje.</p>
                        </div>
                    </body>
                </html>";

            MailMessage mail = new MailMessage
            {
                From = new MailAddress("nutri.app.team@gmail.com"),
                Subject = "Código de verificación de NutriApp",
                Body = bodyHtml,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            mail.To.Add(destinatario);

            SmtpClient client = new SmtpClient
            {
                Credentials = new NetworkCredential("nutri.app.team@gmail.com", "contraseña de aplicacion"), // Cambiar a contraseña generada por Google
                Port = 587,
                EnableSsl = true,
                Host = "smtp.gmail.com"
            };

            try
            {
                await client.SendMailAsync(mail); // Ahora enviará el correo correctamente
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el correo electrónico: " + ex.Message);
            }

            // Retornar el código de verificación para que se pueda almacenar y comparar
            return numeroVerificar;
        }
    }
}
