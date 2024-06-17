
# BirdsNearYou

A GIS web application that tracks bird sightings in an area. Select any state and see the most recent sightings courtesy of the Ebird orginization


## Run Locally

This project uses ASP.NET core and requires the .NET SDK is installed on your machine to run locally. Additionally this application makes use of third part API, Ebird and Flickr, for gathering data. In order to run the application with full functionality you'll require API keys from the following sources:

Flickr: https://www.flickr.com/services/api/misc.api_keys.html

Ebird: https://www.birds.cornell.edu/home/ebird-api-terms-of-use/

Once retrieved these keys can be put in an .env file in the root project folder.

```bash
EBIRD_API_KEY=YourKeyHere
FLICKR_API_KEY=YourKeyHere
```

Once installed you can use the following steps

```bash
  git clone https://github.com/liam2258/BirdsNearYou
```

Go to the project directory

```bash
  cd BirdsNearYou/src/BirdsNearYou
```

Install dependencies

```bash
  dotnet restore
```

Start the server

```bash
  dotnet run
```


## Running Tests

This project uses xUnit for it's testing. After cloning the project tests can be run from the test directory using the following commands.

```bash
  cd BirdsNearYou/tests/BirdsNearYouTests
```

```bash
  dotnet test
```
