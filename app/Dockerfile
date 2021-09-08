FROM mcr.microsoft.com/dotnet/core/sdk:5.0 as build

# Set working directory
WORKDIR /usr/app

# Copy project files and restore nuget packages
COPY ./src  ./src

COPY D3SK.NetCore.sln D3SK.NetCore.sln

RUN dotnet publish -c Release src/ExampleBookstore/Services/ServicesGateway.Api/ServicesGateway.Api.csproj --output ./build

#### Create deployment container
FROM mcr.microsoft.com/dotnet/core/aspnet:5.0

RUN apt-get update && apt-get install -y \
  libmagic-mgc \
  libxext6 \
  libxrender1 \
  fontconfig \
  wget \
  xz-utils \
  libgdiplus \
  libc6

# Set work directory on deployment container
WORKDIR /usr/app

# Copy compilation output from build to deployment container
COPY --from=build /usr/app/build /usr/app

CMD [ "dotnet", "ExampleBookstore.Services.ServicesGateway.Api.dll" ]
