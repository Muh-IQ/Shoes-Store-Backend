# المرحلة الأولى: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# نسخ كل ملفات المشروع
COPY . .

# استعادة الحزم
RUN dotnet restore "ShoesStore_Project/ShoesStore_Project.csproj"

# بناء المشروع
RUN dotnet publish "ShoesStore_Project/ShoesStore_Project.csproj" -c Release -o out

# المرحلة الثانية: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# تشغيل التطبيق
ENTRYPOINT ["dotnet", "ShoesStore_Project.dll"]
