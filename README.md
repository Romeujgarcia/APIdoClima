# APIdoClima

Crie uma API climática que busque e retorne dados meteorológicos.

Neste projeto, em vez de depender de nossos próprios dados meteorológicos, construiremos uma API meteorológica que busca e retorna dados meteorológicos de uma API de terceiros. Este projeto ajudará você a entender como trabalhar com APIs de terceiros, cache e variáveis ​​de ambiente.


![image](https://github.com/user-attachments/assets/0cb14d65-7f35-4dad-919f-4ac62f1eaed5)


Quanto à API do clima a ser usada, você pode usar a sua favorita. Como sugestão, aqui está um link para a API do Visual Crossing , é totalmente GRÁTIS e fácil de usar.

Em relação ao cache na memória, uma recomendação bastante comum é usar o Redis , você pode ler mais sobre isso aqui , e como recomendação, você pode usar o código da cidade inserido pelo usuário como chave e salvar lá o resultado da chamada da API.

Ao mesmo tempo, ao "definir" o valor no cache, você também pode definir um tempo de expiração em segundos (usando o EXsinalizador no SETcomando). Dessa forma, o cache (as chaves) se limpará automaticamente quando os dados estiverem antigos o suficiente (por exemplo, definindo um tempo de expiração de 12 horas).

## Funcionalidades

- Obtenção de dados climáticos para cidades específicas.
- Cache de resultados usando Redis para melhorar a performance.
- Limitação de taxa para controlar o número de requisições por cliente.

## Tecnologias Utilizadas

- ASP.NET Core 6
- C#
- Visual Crossing Weather API
- Redis (para cache)
- HttpClient (para chamadas HTTP)

## Pré-requisitos

Antes de começar, você precisará ter instalado:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Redis](https://redis.io/download) (ou um serviço de Redis em nuvem)
- Uma chave de API da [Visual Crossing Weather API](https://www.visualcrossing.com/weather-api)

## Configuração

1. Clone o repositório:

   ```bash
   git clone https://github.com/romeujgarcia/APIdoClima.git
   cd APIdoClima

   {
  "AppSettings": {
    "APIKey": "SUA_CHAVE_DE_API",
    "RedisConnectionString": "localhost:6379" // ou a string de conexão do seu Redis
  }
}

dotnet run
Testes
Para garantir que a API funcione conforme o esperado, você pode implementar testes unitários e de integração. Utilize o framework de testes do .NET, como o xUnit ou NUnit, para criar seus testes.
GET https://localhost:7284/Weather/são paulo

using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using APIdoClima.Controllers;
using APIdoClima.Services;

public class WeatherControllerTests
{
    [Fact]
    public async Task GetWeather_ReturnsOkResult_WhenCityIsValid()
    {
        // Arrange
        var mockService = new Mock<IWeatherService>();
        mockService.Setup(service => service.GetWeatherAsync("são paulo"))
                   .ReturnsAsync(new WeatherResponse { /* dados de exemplo */ });
        var controller = new WeatherController(mockService.Object);

        // Act
        var result = await controller.GetWeather("são paulo");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }
}
dotnet test

### Melhorias e Correções

1. **Estrutura do `appsettings.json`**: A estrutura do JSON foi movida para a seção de configuração, onde ela pertence.
2. **Formatação**: O README foi formatado para melhorar a legibilidade, com seções claramente definidas e exemplos de código format ados corretamente.
3. **Clareza**: Algumas instruções foram ligeiramente reformuladas para maior clareza e fluidez.
4. **Consistência**: Certifique-se de que todos os links e referências estejam atualizados e funcionais.

Sinta-se à vontade para ajustar ainda mais conforme necessário e adicionar qualquer informação adicional que considere relevante para os usuários do seu projeto! Se precisar de mais assistência, estou aqui para ajudar! ```markdown
## Recursos Adicionais

- [Documentação do ASP.NET Core](https://docs.microsoft.com/aspnet/core/?view=aspnetcore-6.0)
- [Visual Crossing Weather API Documentation](https://www.visualcrossing.com/resources/documentation/weather-api/)
- [Redis Documentation](https://redis.io/documentation)

## Agradecimentos

Agradecemos a todos que contribuíram para o desenvolvimento deste projeto. Sua ajuda e feedback são muito apreciados!

## Considerações Finais

Sinta-se à vontade para adicionar seções adicionais, como "FAQ" ou "Problemas Conhecidos", conforme necessário. Um README abrangente pode fazer uma grande diferença na experiência do usuário e na adoção do seu projeto. Se precisar de mais assistência, estou à disposição!
``` ```markdown
## Recursos Adicionais

- [Documentação do ASP.NET Core](https://docs.microsoft.com/aspnet/core/?view=aspnetcore-6.0)
- [Visual Crossing Weather API Documentation](https://www.visualcrossing.com/resources/documentation/weather-api/)
- [Redis Documentation](https://redis.io/documentation)

## Agradecimentos

Agradecemos a todos que contribuíram para o desenvolvimento deste projeto. Sua ajuda e feedback são muito apreciados!

## Considerações Finais

Sinta-se à vontade para adicionar seções adicionais, como "FAQ" ou "Problemas Conhecidos", conforme necessário. Um README abrangente pode fazer uma grande diferença na experiência do usuário e na adoção do seu projeto. Se precisar de mais assistência, estou à disposição!
