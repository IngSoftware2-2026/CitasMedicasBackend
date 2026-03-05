namespace CitasMedicas.API.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY = "XApiKey";
        private const string ENCRYPTION = "EncryptionKey";
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            //_encryption = encryption;
        }




        public async Task InvokeAsync(HttpContext context)
        {
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(APIKEY);
            var password = appSettings.GetValue<string>(ENCRYPTION);

            //var keyVaultEndpoint = "https://simexpro.vault.azure.net/"; // Replace with your Key Vault URI
            //var azureServiceTokenProvider = new AzureServiceTokenProvider();
            //var keyVaultClient = new KeyVaultClient(
            //    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback)
            //);

            // Check if the path starts with the endpoint to skip API key validation
            var path = context.Request.Path.Value.ToLower();
            if (path.StartsWith("/api/empleado/obtenerimagen"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Path == "/api/Usuarios/Login")
            {
                context.Response.OnStarting(async () =>
                {
                    if (context.Response.StatusCode == 200)
                    {
                        //var secretKey = await keyVaultClient.GetSecretAsync($"{keyVaultEndpoint}secrets/{APIKEY}");
                        //var apiKey = secretKey.Value;
                        //var secretPassword = await keyVaultClient.GetSecretAsync($"{keyVaultEndpoint}secrets/{ENCRYPTION}");
                        //var password = secretPassword.Value;

                        //var encryptedKey = Encryption.Encrypt(apiKey, Encoding.ASCII.GetBytes(password));

                        //context.Response.Headers.Add("Authorization", Convert.ToBase64String(encryptedKey));
                    }

                });

                await _next(context);
            }
            else if (context.Request.Path == "/api/Usuarios/UsuarioCorreo" || context.Request.Path == "/api/Usuarios/CambiarContrasenia" || context.Request.Path == "/api/Duca/GenerarDuca" || context.Request.Path == "/")
            {
                await _next(context);
            }
            else
            {


                if (!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Api Key was not provided ");
                    return;
                }

                try
                {
                    // Retrieve the API key from Azure Key Vault
                    //var secret = await keyVaultClient.GetSecretAsync($"{keyVaultEndpoint}secrets/{APIKEY}");
                    //var apiKey = secret.Value;

                    if (!apiKey.Equals(extractedApiKey))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Unauthorized client");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Failed to retrieve API Key from Azure Key Vault");
                    return;
                }

                await _next(context);
            }

        }
    }
}
