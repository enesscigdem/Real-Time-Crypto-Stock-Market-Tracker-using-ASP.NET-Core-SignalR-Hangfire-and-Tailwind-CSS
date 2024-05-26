document.addEventListener("DOMContentLoaded", function () {
    let stockData = window.stockData;
    let displayCount = 5;

    function getPriceChangeColor(currentPrice, previousPrice) {
        if (currentPrice > previousPrice) {
            return 'price-up';
        } else if (currentPrice < previousPrice) {
            return 'price-down';
        } else {
            return 'price-neutral';
        }
    }

    function renderCards(data) {
        const cardContainer = document.getElementById('cardContainer');
        cardContainer.innerHTML = '';
        data.forEach(stock => {
            const card = document.createElement('div');
            card.id = `stock-card-${stock.Name}`;
            card.className = `border rounded-lg shadow-md p-4 ${getPriceChangeColor(stock.Price, stock.OpenPrice)}`;
            card.innerHTML = `<h2 class="text-xl font-bold">${stock.Name}</h2>
                              <p class="stock-price text-lg" data-stock="${stock.Name}">${stock.Price.toFixed(4)}</p>`;
            cardContainer.appendChild(card);
        });
    }

    function renderTable(data) {
        const stocksTable = document.getElementById('stocksTable');
        stocksTable.innerHTML = '';
        data.forEach(stock => {
            const row = document.createElement('tr');
            row.id = `stock-${stock.Name}`;
            row.className = 'border-t border-gray-200';
            row.innerHTML = `<td class="px-4 py-2">${stock.Name}</td>
                             <td class="stock-open-price px-4 py-2">${stock.OpenPrice.toFixed(2)}</td>
                             <td class="stock-price px-4 py-2 ${getPriceChangeColor(stock.Price, stock.OpenPrice)}" data-stock="${stock.Name}">${stock.Price.toFixed(4)}</td>
                             <td class="stock-change px-4 py-2">${(stock.Price - stock.OpenPrice).toFixed(2)}</td>`;
            stocksTable.appendChild(row);
        });
    }

    function updateStockData(listCount) {
        const displayedData = stockData.slice(0, listCount);
        renderCards(displayedData);
        renderTable(displayedData);
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/stockHub")
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveStockUpdate", (stock, price, color) => {
        const row = document.getElementById(`stock-${stock}`);
        if (row) {
            const priceCell = row.querySelector(".stock-price");
            const previousPrice = parseFloat(priceCell.textContent);
            priceCell.textContent = price.toFixed(4);

            let highlightClass;
            if (price > previousPrice) {
                highlightClass = 'highlight-up';
            } else if (price < previousPrice) {
                highlightClass = 'highlight-down';
            } else {
                highlightClass = 'highlight-neutral';
            }

            priceCell.classList.add(highlightClass);
            setTimeout(() => {
                priceCell.classList.remove(highlightClass);
                priceCell.classList.add('price-neutral');
            }, 1000);
        }

        const card = document.querySelector(`#cardView .stock-price[data-stock="${stock}"]`);
        if (card) {
            const previousPrice = parseFloat(card.textContent);
            card.textContent = price.toFixed(4);

            let highlightClass;
            if (price > previousPrice) {
                highlightClass = 'highlight-up';
            } else if (price < previousPrice) {
                highlightClass = 'highlight-down';
            } else {
                highlightClass = 'highlight-neutral';
            }

            card.parentElement.classList.add(highlightClass);
            setTimeout(() => {
                card.parentElement.classList.remove(highlightClass);
                card.parentElement.classList.add('price-neutral');
            }, 1000);
        }
    });

    connection.start().catch(err => console.error('Connection failed: ', err));

    const toggleView = document.getElementById('toggleView');
    const toggleLabel = document.getElementById('toggleLabel');
    const cardView = document.getElementById('cardView');
    const tableView = document.getElementById('tableView');

    toggleView.addEventListener('change', function () {
        if (toggleView.checked) {
            cardView.classList.remove('hidden');
            tableView.classList.add('hidden');
            toggleLabel.textContent = 'Liste Görünüme Geç';
            document.getElementById('toggleIcon').classList.replace('left-1', 'right-1');
            document.getElementById('toggleBackground').classList.replace('bg-gray-200', 'bg-blue-600');
        } else {
            cardView.classList.add('hidden');
            tableView.classList.remove('hidden');
            toggleLabel.textContent = 'Card Görünüme Geç';
            document.getElementById('toggleIcon').classList.replace('right-1', 'left-1');
            document.getElementById('toggleBackground').classList.replace('bg-blue-600', 'bg-gray-200');
        }
    });

    document.querySelectorAll('button.list-option').forEach(button => {
        button.addEventListener('click', function () {
            document.querySelectorAll('button.list-option').forEach(btn => btn.classList.remove('bg-gray-300'));
            button.classList.add('bg-gray-300');
            displayCount = parseInt(button.dataset.list);
            updateStockData(displayCount);
        });
    });

    document.querySelector('button.list-option[data-list="5"]').classList.add('bg-gray-300');
    updateStockData(displayCount);
});
