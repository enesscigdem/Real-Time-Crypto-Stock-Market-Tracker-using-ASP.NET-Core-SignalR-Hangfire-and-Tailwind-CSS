document.addEventListener("DOMContentLoaded", function () {
    let cryptoData = window.cryptoData;
    let displayCount = 5;

    function getPriceChangeBackground(change) {
        if (parseFloat(change) > 0) {
            return 'bg-green-300'; // Daha açık tonlarda yeşil
        } else if (parseFloat(change) < 0) {
            return 'bg-red-300'; // Daha açık tonlarda kırmızı
        } else {
            return 'bg-gray-200';
        }
    }

    function renderCards(data) {
        const cardContainer = document.getElementById('cardContainer');
        cardContainer.innerHTML = '';
        data.forEach(crypto => {
            const card = document.createElement('div');
            card.id = `crypto-card-${crypto.Symbol}`;
            card.className = `border rounded-lg shadow-md p-4 bg-gray-100`; // Default background color
            card.innerHTML = `<h2 class="text-xl font-bold">${crypto.Name}</h2>
                              <p class="crypto-price text-lg" data-crypto="${crypto.Symbol}">${crypto.Price.toFixed(4)}</p>`;
            cardContainer.appendChild(card);
        });
    }

    function getRandomData() {
        return Array.from({ length: 10 }, () => Math.floor(Math.random() * 100));
    }

    function renderTable(data) {
        const cryptosTable = document.getElementById('cryptosTable');
        cryptosTable.innerHTML = '';
        data.forEach((crypto, index) => {
            const row = document.createElement('tr');
            row.id = `crypto-${crypto.Symbol}`;
            row.className = 'hover:bg-gray-50 rounded-lg';
            row.innerHTML = `
                <td class="px-6 py-4 whitespace-nowrap">${index + 1}</td>
                <td class="px-6 py-4 whitespace-nowrap">${crypto.Symbol}</td>
                <td class="px-6 py-4 whitespace-nowrap">${crypto.Name}</td>
                <td class="px-6 py-4 whitespace-nowrap crypto-price bg-gray-100">${crypto.Price.toFixed(4)}</td>
                <td class="px-6 py-4 whitespace-nowrap">${crypto.Change}</td>
                <td class="px-6 py-4 whitespace-nowrap">${crypto.ChangePercent}</td>
                <td class="px-6 py-4 whitespace-nowrap">${crypto.MarketCap}</td>
                <td class="px-6 py-4 whitespace-nowrap">${crypto.Volume24h}</td>
                <td class="px-6 py-4 whitespace-nowrap">${crypto.CirculatingSupply}</td>
                <td class="px-6 py-4 whitespace-nowrap">
                    <canvas id="chart-${crypto.Symbol}" style="height: 50px;"></canvas>
                </td>
            `;
            cryptosTable.appendChild(row);

            // Grafik Oluştur
            var ctx = document.getElementById(`chart-${crypto.Symbol}`).getContext('2d');
            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: getRandomData().map((_, index) => index + 1),
                    datasets: [{
                        label: 'Price',
                        data: getRandomData(),
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 2,
                        fill: false,
                        tension: 0.4
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Time'
                            }
                        },
                        y: {
                            display: false,
                            title: {
                                display: false
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            display: false
                        }
                    }
                }
            });
        });
    }

    function updateCryptoData(listCount) {
        const displayedData = cryptoData.slice(0, listCount);
        renderCards(displayedData);
        renderTable(displayedData);
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/cryptoHub")
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveCryptoUpdate", (cryptoSymbol, price, change) => {
        const row = document.getElementById(`crypto-${cryptoSymbol}`);
        if (row) {
            const priceCell = row.querySelector(".crypto-price");
            const previousPrice = parseFloat(priceCell.textContent);
            priceCell.textContent = price.toFixed(4);

            let highlightClass = getPriceChangeBackground(change);
            priceCell.className = `px-6 py-4 whitespace-nowrap crypto-price ${highlightClass}`;

            setTimeout(() => {
                priceCell.className = 'px-6 py-4 whitespace-nowrap crypto-price bg-gray-100';
            }, 1000);
        }

        const card = document.querySelector(`#cardView .crypto-price[data-crypto="${cryptoSymbol}"]`);
        if (card) {
            const previousPrice = parseFloat(card.textContent);
            card.textContent = price.toFixed(4);

            let highlightClass = getPriceChangeBackground(change);
            card.parentElement.className = `border rounded-lg shadow-md p-4 ${highlightClass}`;

            setTimeout(() => {
                card.parentElement.className = 'border rounded-lg shadow-md p-4 bg-gray-100';
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
            toggleLabel.textContent = 'Card Görünümüne Geç';
            document.getElementById('toggleIcon').classList.replace('right-1', 'left-1');
            document.getElementById('toggleBackground').classList.replace('bg-blue-600', 'bg-gray-200');
        }
    });

    document.querySelectorAll('button.list-option').forEach(button => {
        button.addEventListener('click', function () {
            document.querySelectorAll('button.list-option').forEach(btn => btn.classList.remove('bg-gray-300'));
            button.classList.add('bg-gray-300');
            displayCount = parseInt(button.dataset.list);
            updateCryptoData(displayCount);
        });
    });

    updateCryptoData(displayCount);
});
