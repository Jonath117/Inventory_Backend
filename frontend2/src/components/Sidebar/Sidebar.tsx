import { NavLink } from "react-router-dom";
import { useState } from "react";

import {
  HomeIcon,
  UsersIcon,
  FolderIcon,
  CalendarIcon,
  DocumentDuplicateIcon,
  ChartPieIcon,
} from "@heroicons/react/24/outline";

const navigation = [
  { name: "Inicio", href: "/", icon: HomeIcon, badge: 5 },
  { name: "Team", href: "/team", icon: UsersIcon },
  { name: "Projects", href: "/projects", icon: FolderIcon, badge: 12 },
  { name: "Calendar", href: "/calendar", icon: CalendarIcon, badge: "20+" },
  { name: "Documents", href: "/documents", icon: DocumentDuplicateIcon },
  { name: "Reports", href: "/reports", icon: ChartPieIcon },
];

const teams = [
  { id: 1, name: "Heroicons", initial: "H" },
  { id: 2, name: "Tailwind Labs", initial: "T" },
  { id: 3, name: "Workcation", initial: "W" },
];

const SideBar = () => {
    const [isOpen, setIsOpen] = useState(false);

  const toggleSidebar = () => {
    setIsOpen(!isOpen);

  }

  return (
    <aside className="flex flex-col h-screen w-64 bg-[#111827] text-white px-4 py-6 shrink-0">
      {/* Logo */}
      <div className="mb-8 px-2 flex items-center justify-between">
        <svg
          className="w-8 h-8 text-indigo-400"
          viewBox="0 0 48 48"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M6 24C6 14.059 14.059 6 24 6"
            stroke="currentColor"
            strokeWidth="5"
            strokeLinecap="round"
          />
          <path
            d="M42 24C42 33.941 33.941 42 24 42"
            stroke="currentColor"
            strokeWidth="5"
            strokeLinecap="round"
          />
          <path
            d="M24 6C33.941 6 42 14.059 42 24"
            stroke="#818cf8"
            strokeWidth="5"
            strokeLinecap="round"
          />
          <path
            d="M24 42C14.059 42 6 33.941 6 24"
            stroke="#818cf8"
            strokeWidth="5"
            strokeLinecap="round"
          />
        </svg>

        <button className="ml-4 p-1 rounded-md hover:bg-[#1a2232] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500" 
          onClick={toggleSidebar}>
              <svg
                className="w-7 h-7 text-gray-400"
                viewBox="0 0 20 20"
                fill="currentColor"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  fillRule="evenodd"
                  clipRule="evenodd"
                  d="M10 18C14.4183 18 18 14.4183 18 10C18 5.58172 14.4183 2 10 2C5.58172 2 2 5.58172 2 10C2 14.4183 5.58172 18 10 18ZM9.29289 6.29289C9.68342 5.90237 10.3166 5.90237 10.7071 6.29289L14.7071 10.2929C15.0976 10.6834 15.0976 11.3166 14.7071 11.7071L10.7071 15.7071C10.3166 16.0976 9.68342 16.0976 9.29289 15.7071C8.90237 15.3166 8.90237 14.6834 9.29289 14.2929L12.5858 11L9.29289 7.70711C8.90237 7.31658 8.90237 6.68342 9.29289 6.29289Z"
                fill="currentColor"
                />
              </svg>
          
        </button>
      </div>

      {/* Main Navigation */}
      <nav className="flex flex-col gap-1">
        {navigation.map((item) => (
          <NavLink
            key={item.name}
            to={item.href}
            end={item.href === "/"}
            className={({ isActive }) =>
              `flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-colors duration-150 ${
                isActive
                  ? "bg-[#1f2937] text-white"
                  : "text-gray-400 hover:bg-[#1a2232] hover:text-white"
              }`
            }
          >
            {({ isActive }) => (
              <>
                <item.icon
                  className={`w-5 h-5 shrink-0 ${isActive ? "text-white" : "text-gray-400"}`}
                />
                <span className="flex-1">{item.name}</span>
                {item.badge !== undefined && (
                  <span className="text-xs font-semibold bg-[#1a2232] border border-[#2d3748] text-gray-300 rounded-full px-2 py-0.5 min-w-[1.5rem] text-center">
                    {item.badge}
                  </span>
                )}
              </>
            )}
          </NavLink>
        ))}
      </nav>

      {/* Teams Section */}
      <div className="mt-8">
        <p className="text-xs font-semibold text-gray-500 uppercase tracking-wider px-3 mb-3">
          Your teams
        </p>
        <div className="flex flex-col gap-1">
          {teams.map((team) => (
            <NavLink
              key={team.id}
              to={`/teams/${team.name.toLowerCase().replace(/\s+/g, "-")}`}
              className="flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium text-gray-400 hover:bg-[#1a2232] hover:text-white transition-colors duration-150"
            >
              <span className="w-6 h-6 rounded-md bg-[#1f2937] border border-[#374151] flex items-center justify-center text-xs font-semibold text-gray-300 shrink-0">
                {team.initial}
              </span>
              {team.name}
            </NavLink>
          ))}
        </div>
      </div>
    </aside>
  );
};

export default SideBar;
