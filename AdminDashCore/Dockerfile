# 1. Bazaviy image: runtime uchun
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# 2. Build uchun image: SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish AdminDashCore/AdminDashCore.csproj -c Release -o /app/publish

# 3. Final image: faqat publish qilingan fayllar
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AdminDashCore.dll"]
