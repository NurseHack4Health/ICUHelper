import React from 'react';
import logo from './logo.svg';
import './App.css';
import { Login } from "./components/login/index";

function App() {
  return React.createElement(
    'div',
    { className: 'App' },
    React.createElement(Login, null)
  );
}

export default App;