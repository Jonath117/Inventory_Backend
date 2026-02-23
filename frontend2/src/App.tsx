import './App.css'
import { BrowserRouter, Route, Routes } from "react-router-dom";
import MainLayout from './layout/MainLayout.tsx';
import { DashboardPage } from './features/dashboard/pages/inventoryDashboardPage.tsx';
import { SelectedCompanyPages } from './features/company/pages/SelectedCompanyPages.tsx';

function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route element={<MainLayout />}>
          <Route path="/" element={<SelectedCompanyPages />} />
          <Route path="/dashboard" element={<DashboardPage />} />
        </Route>

        {/*<Route path="/login" element={<Login />} />*/}
      </Routes>
    </BrowserRouter>
  )
}

export default App
