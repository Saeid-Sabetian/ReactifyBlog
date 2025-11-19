import React from 'react';
import { AuthForm } from '../components/AuthForm';

export const AuthPage: React.FC = () => {
  return (
    <div className="auth-page-container">
      <div className="background-image-blurred"></div>
      <div className="auth-form-wrapper">
        <AuthForm />
      </div>
    </div>
  );
};
