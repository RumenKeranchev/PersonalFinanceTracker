import { CategoryScale, Chart, Legend, LinearScale, LineElement, PointElement, Title, Tooltip, type ChartData, type ChartOptions } from "chart.js";
import { useEffect, useState } from "react";
import { Container } from "react-bootstrap";
import { Line } from "react-chartjs-2";
import type { Dashboard } from "../../api";
import axios from "axios";

Chart.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
);

const Home = () => {
    const [data, setData] = useState<ChartData<"line">>({ datasets: [] });

    const options: ChartOptions<"line"> = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                position: 'top' as const,
            },
        },
    };



    useEffect(() => {
        async function fetchData() {
            try {
                const response = await axios.get("/reporting");

                if (response.status === 200) {
                    const respData = response.data as Dashboard;

                    setData({
                        labels: respData.labels,
                        datasets: [
                            {
                                label: respData.datasets[0].label,
                                data: respData.datasets[0].data,
                                borderColor: 'rgb(255, 99, 132)',
                                backgroundColor: 'rgba(255, 99, 132, 0.5)',
                            },
                            {
                                label: respData.datasets[1].label,
                                data: respData.datasets[1].data,
                                borderColor: 'rgb(53, 162, 235)',
                                backgroundColor: 'rgba(53, 162, 235, 0.5)',
                            }
                        ]
                    });
                }
            } catch {
                console.error('Error fetching data');
            }
        }

        fetchData();
    }, []);

    return (
        <Container fluid>
            <title>Dashboard</title>
            <h2 className="text-center">Dashboard</h2>

            <div className="d-flex justify-content-evenly gap-3 mt-5">
                <div style={{ width: "66%", height: 400, border: "1px solid cyan" }} className="align-content-center">
                    {
                        data.datasets.length > 0 &&
                        <Line options={options} data={data} />
                    }
                </div>
            </div>
        </Container >
    );
};

export default Home;