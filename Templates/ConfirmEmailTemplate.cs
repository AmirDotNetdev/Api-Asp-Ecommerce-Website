using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Templates
{
    public class ConfirmEmailTemplate
    {
        public static string EmailLinkTemplate(string callbackUrl)
        {
            var msgStart = @"
                <html>
                    <head>
                        <meta charset='utf-8' />
                        <title></title>
                        <style>
                            @mixin media()
                            {
                                @media(min-width: 768px)
                                {
                                    @content   
                                }

                            }
                            body, html
                            {
                                font-family: 'Vollkorn', serif;
	                            font-weight: 400;
	                            line-height: 1.3;
	                            font-size: 16px;
                            }
                            
                            header, main, footer
                            {
                                max-width: 960px;
                                margin: 0 auto;
                            }
                            .title
                            {
                                font-size: 24px;
	                            color: black;  
	                            text-align: center;
	                            font-weight: 700;
	                            color: #181818;
	                            text-shadow: 0px 2px 2px #a6f8d5;
	                            position: relative;
	                            margin: 0 0 20px 0;
	                            
	                            @include media 
                                {
	                            	font-size: 30px;
	                            }
                            }
                            table
                            {
                                width:100%;
                                border: 1px solid;
                                @include media
                                {
                                    font-size:10px;
                                }
                            }
                            th
                            {
                                background-color: gray;
                            }
                            td, th
                            {
                                text-align: center;
                                vertical-align: middle;
                            }
                            .margin-top
                            {
                                margin-top: 25px;
                            }
                        </style>
                    </head>
                    <body>";

            var emailBody = $"Please confirm your email address " +
               $"<a href=\"#URL#\">Click here</a>";
            var body = emailBody.Replace("#URL#", callbackUrl);
            string msgEnd = "</body></html>";

            return msgStart + body+ msgEnd;

        }
    }
}