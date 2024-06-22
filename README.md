
# OneWay

## Overview
OneWay is a software tool designed for the systematic optimization and calculation of trip costs, covering various modes of transport. The application leverages multiple technologies and APIs to provide accurate and efficient trip planning.

## Features
- **Multi-modal Transport Optimization**: Plan trips using different modes of transport.
- **Cost Calculation**: Accurate calculation of trip costs.
- **API Integration**: Integration with [Yandex](https://yandex.com/maps-api/products/geocoder-api?lang=en) and [Graphhopper](https://docs.graphhopper.com/#tag/Routing-API) for routing
- **User Interface**: User-friendly interface designed with WPF.
- **Database**: SQLite for local data storage.
- **Parsing**: Getting fuel price and getting cars data.
## Technologies Used
- **C#**: Core programming language.
- **WPF (Windows Presentation Foundation)**: For creating a rich user interface.
- **SQLite**: Local database for storing trip data.
- **APIs**: 
  - **[Graphhopper](https://docs.graphhopper.com/#tag/Routing-API)**: For create a trip route.
  - **[Yandex](https://yandex.com/maps-api/products/geocoder-api?lang=en)**: For determine the coordinates of the route points.
- **Parser**: 
    - **[Belorusneft](https://azs.belorusneft.by/sitebeloil/ru/center/azs/center/fuelandService/price/)**: For getting fuel price.
    - **[Drom](https://www.drom.ru/catalog/)**: For getting cars data.
- **Figma**: Design tool used for the UI/UX design.
- **Dr.Explain**: For create a help system.


## Usage
1. Start the application.
2. Register or authenticate in the system.
3. Create a route on the Routes tab.
4. Create a car on the Cars tab.
5. Enter the details of your trip, including the route, and select the desired car.


