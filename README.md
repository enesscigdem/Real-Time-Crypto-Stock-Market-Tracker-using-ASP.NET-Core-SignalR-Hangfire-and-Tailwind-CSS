# Crypto and Stock Market Tracker

This project is a comprehensive web application for tracking real-time stock and cryptocurrency prices. Built with modern web technologies, it offers dynamic and responsive user interfaces using both list and card views. 

## Technologies Used

- **ASP.NET Core MVC 7.0:** A web framework for building dynamic, scalable, and high-performance web applications.
- **Finnhub API:** Used for fetching stock market data for 10 predefined stocks, updated every second.
- **Yahoo Finance:** Scraped data for 100 cryptocurrencies, including their prices and historical data, updated every second.
- **Hangfire:** Used for background processing. We used Hangfire server and dashboard for monitoring.
- **SignalR:** Implemented for real-time web functionality, providing instant updates for stock and cryptocurrency prices.
- **Tailwind CSS:** Utilized for styling the application, ensuring a modern and responsive design.

## Features

- **Real-Time Data Updates:** Stock and cryptocurrency data are updated every second.
- **Background Processing:** Hangfire handles background tasks efficiently, visible through the Hangfire dashboard.
- **Real-Time Notifications:** SignalR enables real-time updates, ensuring users receive the latest data without refreshing the page.
- **Dual View Modes:** Users can toggle between list and card views for convenience.
- **AI Recommendations:** Provides simple buy, sell, or hold recommendations based on recent price trends.
- **Responsive Design:** Tailwind CSS ensures the application looks great on all devices.

## Getting Started

Follow these steps to set up and run the project on your local machine.

### Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Node.js and npm](https://nodejs.org/) (for Tailwind CSS)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or any preferred code editor

### Installation

1. **Clone the Repository:**

   ```sh
   git clone https://github.com/your-username/crypto-stock-market-tracker.git
   cd crypto-stock-market-tracker
   ```

2. **Install Dependencies:**

   Restore the .NET dependencies:
   
   ```sh
   dotnet restore
   ```

   Install npm packages for Tailwind CSS:
   
   ```sh
   npm install
   ```

3. **Build the Project:**

   ```sh
   dotnet build
   ```

4. **Configure API Keys:**

   Add your Finnhub API key and any other necessary configurations in `appsettings.json`:

   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "Finnhub": {
       "ApiKey": "your_finnhub_api_key"
     }
   }
   ```

5. **Run Database Migrations:**

   ```sh
   dotnet ef database update
   ```

6. **Run the Application:**

   ```sh
   dotnet run
   ```

   Alternatively, you can start the application from Visual Studio by pressing `F5`.

### Usage

Once the application is running, you can access it at `http://localhost:5000`.

- **Hangfire Dashboard:** Navigate to `http://localhost:5000/hangfire` to view and manage background jobs.
- **Real-Time Data:** The homepage will display real-time stock and cryptocurrency data with options to switch between list and card views.

## Project Structure

- **Controllers:** Handles incoming requests and returns appropriate responses.
- **Models:** Defines the structure of the data used in the application.
- **Views:** Contains the HTML and Razor files that make up the user interface.
- **wwwroot:** Contains static files such as JavaScript, CSS, and images.
- **Services:** Contains classes that handle business logic, including data fetching from APIs and background job scheduling.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a new branch: `git checkout -b feature-name`.
3. Make your changes and commit them: `git commit -m 'Add some feature'`.
4. Push to the branch: `git push origin feature-name`.
5. Open a pull request.

## License

This project is licensed under the MIT License. See the `LICENSE` file for more information.

## Contact

For any questions or inquiries, please reach out to:

- **Email:** enescigdeem@gmail.com
- **GitHub:** [enesscigdem](https://github.com/enesscigdem)
