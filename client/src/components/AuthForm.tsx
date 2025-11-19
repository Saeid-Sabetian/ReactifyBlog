import React, { useState } from 'react';
import { authService } from '../services/authService';
import { useNavigate } from 'react-router-dom';

export const AuthForm: React.FC = () => {
  const navigate = useNavigate();
  const [isLogin, setIsLogin] = useState(true); // true for login, false for register
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    confirmPassword: '',
    rememberMe: false,
  });

  const toggleAuthMode = (e: React.MouseEvent) => {
    e.preventDefault();
    setIsLogin((prev) => !prev);
    setFormData({ email: '', password: '', confirmPassword: '', rememberMe: false });
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (isLogin) {
      console.log('Attempting Login...', { email: formData.email, password: formData.password, rememberMe: formData.rememberMe });
      const response = await authService.login({ email: formData.email, password: formData.password, rememberMe: formData.rememberMe });
      if (response.Error) {
        console.error('Login failed:', response.Error);
        alert(`Login failed: ${response.Error.Message}`);
      } else {
        console.log('Login successful:', response.Data);
        alert('Login successful!');
        // TODO: Redirect user or update authentication context
      }
    } else {
      console.log('Attempting Register...', { email: formData.email, password: formData.password, confirmPassword: formData.confirmPassword });
      if (formData.password !== formData.confirmPassword) {
        alert('Passwords do not match!');
        return;
      }
      const response = await authService.register({ email: formData.email, password: formData.password, confirmPassword: formData.confirmPassword });
      if (response.Error) {
        console.error('Registration failed:', response.Error);
        alert(`Registration failed: ${response.Error.Message}`);
      } else {
        console.log('Registration successful:', response.Data);
        alert('Registration successful!');
        // TODO: Redirect user to login or update authentication context
        setIsLogin(true); // Switch to login after successful registration
      }
    }
  };

  return (
    <form onSubmit={handleSubmit} className="auth-form">
      <h2 className="text-center mb-4">{isLogin ? 'Login' : 'Register'}</h2>
      <button onClick={() => navigate(-1)} className="btn btn-secondary mb-3">Home</button>
      <div className="form-group">
        <label htmlFor="email">Email address</label>
        <input
          type="email"
          className="form-control"
          id="email"
          name="email"
          placeholder="Enter email"
          value={formData.email}
          onChange={handleInputChange}
          required
        />
      </div>
      <div className="form-group">
        <label htmlFor="password">Password</label>
        <input
          type="password"
          className="form-control"
          id="password"
          name="password"
          placeholder="Password"
          value={formData.password}
          onChange={handleInputChange}
          required
        />
      </div>
      {!isLogin && (
        <div className="form-group">
          <label htmlFor="confirmPassword">Confirm Password</label>
          <input
            type="password"
            className="form-control"
            id="confirmPassword"
            name="confirmPassword"
            placeholder="Confirm Password"
            value={formData.confirmPassword}
            onChange={handleInputChange}
            required
          />
        </div>
      )}
      {isLogin && (
        <div className="form-group form-check">
          <input
            type="checkbox"
            className="form-check-input"
            id="rememberMe"
            name="rememberMe"
            checked={formData.rememberMe}
            onChange={handleInputChange}
          />
          <label className="form-check-label" htmlFor="rememberMe">Remember Me</label>
        </div>
      )}
      <button type="submit" className="btn btn-main btn-block mt-4">
        {isLogin ? 'Login' : 'Register'}
      </button>
      <p className="text-center mt-3">
        {isLogin ? "Don't have an account?" : "Already have an account?"}
        <a href="#" onClick={toggleAuthMode} className="ml-2 text-color">
          {isLogin ? 'Create one' : 'Login here'}
        </a>
      </p>
    </form>
  );
};
