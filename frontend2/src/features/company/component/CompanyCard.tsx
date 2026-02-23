import type { Company } from "../types/company";
import {useNavigate} from "react-router-dom";

interface Props {
    company: Company;
}

export const CompanyCard = ({ company }: Props) => {
    const Navigate = useNavigate();

    const handleSelect = () => {
        localStorage.setItem("activeCompany", JSON.stringify(company));
        Navigate("/dashboard");
        
    };

    return (
        <div 
        onClick={handleSelect}
        className="flex items-center justify-between p-5 cursor-pointer hover:bg-blue-50 transition-all duration-200"
        >
            <div >
                <h2 className="text-lg font-semibold text-gray-800">
                    {company.name}
                </h2>
                <p className="text-sm text-gray-500">
                    Seleccionar Empresa
                </p>
            </div>

            <span className="text-blue-600 text-xl font-bold">
                →
            </span>

        </div>
    )
};