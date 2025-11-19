import React from 'react';
import { Link } from 'react-router-dom';

const NotFound: React.FC = () => {
  return (
    <section className="page-404">
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-md-12">
            <a href="index.html">
              <img src="/src/assets/images/logo.png" alt="logo" />
            </a>
            <h1>404</h1>
            <h2>Page Not Found</h2>
            <p>The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.</p>
            <Link to="/" className="btn btn-main"><i className="tf-ion-android-arrow-back"></i> Go to Home</Link>
          </div>
        </div>
      </div>
    </section>
  );
};

export default NotFound;
