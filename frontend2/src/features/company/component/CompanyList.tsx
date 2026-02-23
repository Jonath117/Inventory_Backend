//aqui creare el componente en forma de lista para usarlo en la page SelectedCompanyPages
import { CompanyCard } from "./CompanyCard";
import type { Company } from "../types/company";

interface Props {
    companies: Company[];
}

export const CompanyList = ({ companies }: Props) => {
    return (
        <div className="max-w-2xl mx-auto mt-8 bg-white rounded-2xl shadow-lg divide-y">
            {companies.map((company) => (
                <CompanyCard key={company.id} company={company} />
            ))}
        </div>
    );
}
