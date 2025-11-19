import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import App from './pages/index.tsx';
import Blog from './components/Blog';
import NotFound from './components/NotFound';
import { AuthPage } from './pages/AuthPage';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <Router>
      <Routes>
        <Route path="/" element={<App />}>
          <Route index element={<Blog />} />
        </Route>
        <Route path="/auth" element={<AuthPage />} />
        <Route path="*" element={<NotFound />} />
      </Routes>    
    </Router>
  </React.StrictMode>
);
