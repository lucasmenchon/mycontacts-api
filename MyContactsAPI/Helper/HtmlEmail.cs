using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace MyContactsAPI.Helper
{
    public class HtmlEmail
    {
        public static string GenerateVerificationEmailHTML(string verification)
        {
            return $@"
             <!DOCTYPE html>
                <html lang='en'>

                <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>MyContacts - Verificação da Conta</title>
                <style>
       
                    body {{
                            font - family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}

                    .container {{
                            max - width: 600px;
                        margin: 20px auto;
                        background-color: #ffffff;
                        border-radius: 10px;
                        padding: 20px;
                        box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
                    }}

                    .header {{
                            text - align: center;
                        margin-bottom: 20px;
                    }}

                    .header h1 {{
                        color: #333333;
                        font-size: 28px;
                        font-weight: bold;
                        margin: 0;
                    }}

                    .content {{
                        padding: 20px;
                        background-color: #f9f9f9;
                        border-radius: 10px;
                        margin-bottom: 20px;
                    }}

                    .content p {{
                        color: #666666;
                        font-size: 16px;
                        margin: 0;
                    }}

                    .content strong {{
                        color: #333333;
                        font-weight: bold;
                    }}

                    .footer {{
                        text - align: center;
                        color: #999999;
                        font-size: 14px;
                    }}

                    .footer a {{
                        color: #007bff;
                        text-decoration: none;
                        font-weight: bold;
                    }}

                    .footer a:hover {{
                            text - decoration: underline;
                    }}
                </style>
            </head>

            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>MyContacts - Verificação da Conta</h1>
                    </div>
                    <div class='content'>
                        <p>Olá,</p>
                        <p>Clique no link abaixo para verificar sua conta no MyContacts:</p>
                        <p><a href='#' style='color: #007bff; font-weight: bold;'>Verificar sua conta agora</a></p>
                    </div>
                    <div class='footer'>
                        <p>Este é um e-mail automático, por favor, não responda.</p>
                    </div>
                </div>
            </body>

            </html>";
        }
    }
}
