interface Props {
    company: {
        id: string;
        name: string;
        nit: string;
    };
}

export const CompanyCard = ({ company }: Props) => {
    const handleSelect = () => {
        //guardar empresa activa
        
    };

    return (
        <div className="company-card">
            <h3>{company.name}</h3>
            <p>NIT: {company.nit}</p>
            <button onClick={handleSelect}>Seleccionar</button>
        </div>
    )
};