import './App.css'
import { BrowserRouter, Route, Routes } from "react-router-dom";
import MainLayout from './layout/MainLayout.tsx';
import { DashboardPage } from './features/dashboard/pages/inventoryDashboardPage.tsx';
import { SelectedCompanyPages } from './features/company/pages/SelectedCompanyPages.tsx';
import { CompanyProvider } from './features/company/pages/CompanyContext.tsx';

function App() {

  return (
    <BrowserRouter>
    <CompanyProvider>
      <Routes>
        <Route element={<MainLayout />}>
          <Route path="/" element={<SelectedCompanyPages />} />
          <Route path="/dashboard" element={<DashboardPage />} />
        </Route>
      </Routes>
    </CompanyProvider>
    </BrowserRouter>
  )
}

export default App
