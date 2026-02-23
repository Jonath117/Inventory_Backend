import { useEffect, useState } from "react";
import { getDashboard } from "../../../services/DashboardServices";


interface DashboardData {
    totalProducts: number;
    totalStockQuantity: number;
    lowStockAlerts: number;

}

export const DashboardPage = () => {
    const [dashboard, setDasboard] = useState<DashboardData | null>(null);
    const [loading, setLoading] = useState(true);

    const companyId = JSON.parse(localStorage.getItem("activeCompany") || "{}").id;

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

    if(loading) return <div className="flex h-screen text-center items-center justify-center text-4xl">Cargando...</div>;

    return(
        <div className="text-center">
            <h1 className="text-3xl font-bold text-blue-600">Dashboard de Inventario</h1>
                <p className="text-lg text-gray-800">Total de productos: {dashboard?.totalProducts}</p>
                <p className="text-lg text-gray-800">Stock total: {dashboard?.totalStockQuantity}</p>
                <p className="text-lg text-gray-800">Alertas de bajo stock: {dashboard?.lowStockAlerts}</p>
        </div>
    )
}
