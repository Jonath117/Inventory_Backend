import { useEffect, useState } from "react";
import { CompanyList } from "../component/CompanyList";
import { getCompanies } from "../../../services/ChooseCompnay";
import type { Company } from "../types/company";



export const SelectedCompanyPages = () => {
    const[companies, setCompanies] = useState<Company[]>([]);
    const[loading, setLoading] = useState<boolean>(false);

    useEffect(() => {
        setLoading(true);
        getCompanies()
            .then((data) => {
                setCompanies(data);
                setLoading(false);
            })
            .catch((error) => {
                console.error("Error al obtener las empresas:", error);
                setLoading(false);
            });
    }, []);

    if(loading) return <div className="flex h-screen text-center items-center justify-center text-4xl">Cargando...</div>;

    return (
        <div className="min-h-screen bg-gray-50 flex flex-col items-center px-6 py-12">
            
            <div className="text-center mb-10">
                <h1 className="text-4xl font-extrabold text-gray-900">
                    Selecciona una Empresa
                </h1>
                <p className="text-gray-500 mt-2">
                    Elige la empresa con la que deseas trabajar
                </p>
            </div>

            <div className="w-full max-w-6xl">
                <CompanyList companies={companies} />
            </div>
        </div>
    )
}

