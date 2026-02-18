import { useEffect, useState } from "react";
import { getDashboard } from "../../../services/DashboardServices";

interface DashboardData {
    totalProducts: number;
    totalWarehouses: number;
    totalStockQuantity: number;
    lowStockAlerts: number;

}

export const DashboardPage = () => {
    const [dashboard, setDasboard] = useState<DashboardData | null>(null);
    const [loading, setLoading] = useState(true);

    const companyId = 1; // Reemplaza con el ID de la empresa seleccionada

    useEffect(() => {
        getDashboard(companyId)
            .then((data) => {
                setDasboard(data);
                setLoading(false);
            })
            .catch((error) => {
                console.error("Error al obtener el dashboard:", error);
                setLoading(false);
            });
    }, [companyId]);

    if(loading) return <div>Cargando...</div>;

    return(
        <div>
            <h1>Dashboard</h1>
                <p>Total de productos: {dashboard?.totalProducts}</p>
                <p>Total de almacenes: {dashboard?.totalWarehouses}</p>
                <p>Total de cantidad en stock: {dashboard?.totalStockQuantity}</p>
                <p>Alertas de bajo stock: {dashboard?.lowStockAlerts}</p>
        </div>
    )
}
