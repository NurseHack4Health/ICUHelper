/*!

=========================================================
* Material Dashboard React - v1.8.0
=========================================================

* Product Page: https://www.creative-tim.com/product/material-dashboard-react
* Copyright 2019 Creative Tim (https://www.creative-tim.com)
* Licensed under MIT (https://github.com/creativetimofficial/material-dashboard-react/blob/master/LICENSE.md)

* Coded by Creative Tim

=========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

*/
// @material-ui/icons
import Dashboard from "@material-ui/icons/Dashboard";
import Person from "@material-ui/icons/Person";
import LibraryBooks from "@material-ui/icons/LibraryBooks";
// core components/views for Admin layout
import DashboardPage from "views/Dashboard/Dashboard.js";
import Patients from "views/Patients/Patients.js";
import Supplies from "views/Supplies/Supplies.js";
import Chat from "views/Chat/Chat.js";

const dashboardRoutes = [
  {
    path: "/dashboard",
    name: "Dashboard",
    rtlName: "لوحة القيادة",
    icon: Dashboard,
    component: DashboardPage,
    layout: "/admin"
  },
  {
    path: "/patients",
    name: "Patients",
    rtlName: "ملف تعريفي للمستخدم",
    icon: Person,
    component: Patients,
    layout: "/admin"
  },
  {
    path: "/supplies",
    name: "Supplies",
    rtlName: "قائمة الجدول",
    icon: "content_paste",
    component: Supplies,
    layout: "/admin"
  },
  {
    path: "/chat",
    name: "Chat",
    rtlName: "طباعة",
    icon: LibraryBooks,
    component: Chat,
    layout: "/admin"
  }
];

export default dashboardRoutes;
