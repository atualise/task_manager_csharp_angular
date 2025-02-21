# Instalação
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.AspNetCore.Mvc
dotnet add package Microsoft.AspNetCore.App
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package ClosedXML
dotnet add package xUnit
dotnet add package Moq
dotnet add package Microsoft.IdentityModel.Tokens
dotnet add package System.IdentityModel.Tokens.Jwt
dotnet add package BCrypt.Net-Next
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.EntityFrameworkCore.Sqlite


# Criação do banco de dados
dotnet ef database update --project TaskService
