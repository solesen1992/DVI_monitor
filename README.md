# DVI monitor
A program that can monitor temperature and humidity in the warehouse and outside. Additionally, the program should monitor items that fall below the minimum threshold and exceed the maximum threshold. There is also a clock that shows the time for different places in the world (Singapore, London, and Denmark).

Furthermore, there was a request to include news in the program, which is implemented by displaying the headline of the latest news article from nordjyske.dk

## DVI Monitor provides the following information via a Web Service:
- Outdoor humidity
- Outdoor temperature
- Warehouse humidity
- Warehouse temperature
- List of the most sold items of the day
- List of items that are over the maximum in the warehouse
- List of items that are under the minimum in the warehouse

The DVI monitor was part of a group project in 2023.

# Screenshot of the console program
<img width="726" alt="DVI monitor screenshot" src="https://github.com/solesen1992/DVI_monitor/assets/123094773/445a31a3-5d99-4c04-a66a-ec4e0f76aaa9">

# Documentation
The program monitors various types of information. Different methods are used to achieve various tasks, which will be described here.

## Static Variables
We started by creating a static variable called ds. This is a SOAP client that we use to connect to 'DVIService', which we use to fetch our data.

Additionally, we have some static variables TimeZoneInfo, where we define the different time zones so that we can later have the clock display the correct times for the various zones.

## Methods
### Warehouse Status
The warehouseStatus method fetches and prints information about the warehouse status. This includes items below the minimum, items above the maximum, and the most sold items. It prints them in different colors, making it easier to identify the different statistics.

### Temperature
Displays and prints temperature and humidity. It does this both indoors and outdoors, listing them below each other so you can easily get an overview of the temperature and humidity both indoors and outdoors.

### Time
Creates a clock that shows the time for Singapore, London, and Denmark. It updates every second. Additionally, it displays the date and the day of the week.

### Divider
Prints a vertical line down the middle of the screen, dividing the screen and making it more organized.

### WriteInBlue
This method takes text as input and prints it in blue text.

## NewsUpdates
The newsUpdates method fetches XML data from nordjyske.dk and prints the latest news. It updates along with temperature and warehouse status every 5 minutes. This is displayed at the bottom of the screen.

## DVI Service API
We use a DVI Service API, which provides the following methods:

- OutdoorHumidity(): Returns outdoor humidity as a double. This value represents the percentage of humidity.
- StockHumidity(): Returns indoor humidity in the warehouse. Like outdoor, this is a double value representing the percentage of humidity.
- OutdoorTemp(): Returns outdoor temperature as a double value. The temperature is in Celsius.
- StockTemp(): Returns the temperature in the warehouse as a double value. The temperature is in Celsius.
- StockItemsMostSold(): Returns an array of strings with the most sold items of the day.
- StockItemsOverMax(): Returns an array of strings containing items that exceed the maximum warehouse status.
- StockItemsUnderMin(): Returns an array of strings containing items with low warehouse status.

These methods are used in the program to fetch the necessary data about the warehouse, and then format it in our program to appear as we want.
