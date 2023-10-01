# About the project

This is a simple ASP.Net Core WebAPI Application.
Application uses API of the CDEK transport company to calculate delivery price.

## Build with

- ASP.NET 6.0

## REST API

The REST API to the app is described below.

### Get delivery price of a parcel

`GET api/Delivery/`

with query parameters:
string weight, string height, string length, string width, string from, string to

weight - weight of a parcel in kg,
height - height of a parcel in mm,
length - length of a parcel in mm,
width - width of a parcel in mm,
from - name of a city to deliver from,
to - name of a city to deliver to
