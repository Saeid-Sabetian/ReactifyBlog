import React from 'react';
import Header from '../components/Header';

const App: React.FC = () => {
  return (
    <>
      <Header />

      {/* Header Close */}

      <div className="main-wrapper ">
        <section className="page-title bg-1">
          <div className="container">
            <div className="row">
              <div className="col-md-12 col-12">
                <div className="block text-center">
                  <span className="text-white">Our blog</span>
                  <h1 className="text-capitalize mb-4 text-lg">Blog articles</h1>
                  <ul className="list-inline">
                    <li className="list-inline-item"><a href="index.html" className="text-white">Home</a></li>
                    <li className="list-inline-item"><span className="text-white">/</span></li>
                    <li className="list-inline-item"><a href="#" className="text-white-50">Our blog</a></li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </section>

        <section className="section blog-wrap bg-gray">
          <div className="container">
            <div className="row ">
              <div className="col-lg-6 col-md-6 mb-5">
                <div className="blog-item">
                  <img src="/src/assets/images/blog/1.jpg" alt="" className="img-fluid rounded"/>

                  <div className="blog-item-content bg-white p-5">
                    <div className="blog-item-meta bg-gray py-1 px-2">
                      <span className="text-muted text-capitalize mr-3"><i className="ti-pencil-alt mr-2"></i>Creativity</span>
                      <span className="text-muted text-capitalize mr-3"><i className="ti-comment mr-2"></i>5 Comments</span>
                      <span className="text-black text-capitalize mr-3"><i className="ti-time mr-1"></i> 28th January</span>
                    </div> 

                    <h3 className="mt-3 mb-3"><a href="blog-single.html">Improve design with typography?</a></h3>
                    <p className="mb-4">Non illo quas blanditiis repellendus laboriosam minima animi. Consectetur accusantium pariatur repudiandae!</p>

                    <a href="blog-single.html" className="btn btn-small btn-main btn-round-full">Learn More</a>
                  </div>
                </div>
              </div>

              <div className="col-lg-6 col-md-6 mb-5">
                <div className="blog-item">
                  <img src="/src/assets/images/blog/2.jpg" alt="" className="img-fluid rounded"/>

                  <div className="blog-item-content bg-white p-5">
                    <div className="blog-item-meta bg-gray py-1 px-2">
                      <span className="text-muted text-capitalize mr-3"><i className="ti-pencil-alt mr-2"></i>Design</span>
                      <span className="text-muted text-capitalize mr-3"><i className="ti-comment mr-2"></i>5 Comments</span>
                      <span className="text-black text-capitalize mr-3"><i className="ti-time mr-1"></i> 28th January</span>
                    </div>   

                    <h3 className="mt-3 mb-3"><a href="blog-single.html">Interactivity connect consumer</a></h3>
                    <p className="mb-4">Non illo quas blanditiis repellendus laboriosam minima animi. Consectetur accusantium pariatur repudiandae!</p>

                    <a href="blog-single.html" className="btn btn-small btn-main btn-round-full">Learn More</a>
                  </div>
                </div>
              </div>

              <div className="col-lg-6 col-md-6 mb-5">
                <div className="blog-item">
                  <img src="/src/assets/images/blog/3.jpg" alt="" className="img-fluid rounded"/>

                  <div className="blog-item-content bg-white p-5">
                    <div className="blog-item-meta bg-gray py-1 px-2">
                      <span className="text-muted text-capitalize mr-3"><i className="ti-pencil-alt mr-2"></i>Community</span>
                      <span className="text-muted text-capitalize mr-3"><i className="ti-comment mr-2"></i>5 Comments</span>
                      <span className="text-black text-capitalize mr-3"><i className="ti-time mr-1"></i> 28th January</span>
                    </div>  

                    <h3 className="mt-3 mb-3"><a href="blog-single.html">Marketing Strategy to bring more affect</a></h3>
                    <p className="mb-4">Non illo quas blanditiis repellendus laboriosam minima animi. Consectetur accusantium pariatur repudiandae!</p>

                    <a href="blog-single.html" className="btn btn-small btn-main btn-round-full">Learn More</a>
                  </div>
                </div>
              </div>
              <div className="col-lg-6 col-md-6 mb-5">
                <div className="blog-item">
                  <img src="/src/assets/images/blog/4.jpg" alt="" className="img-fluid rounded"/>

                  <div className="blog-item-content bg-white p-5">
                    <div className="blog-item-meta bg-gray py-1 px-2">
                      <span className="text-muted text-capitalize mr-3"><i className="ti-pencil-alt mr-2"></i>Marketing</span>
                      <span className="text-muted text-capitalize mr-3"><i className="ti-comment mr-2"></i>5 Comments</span>
                      <span className="text-black text-capitalize mr-3"><i className="ti-time mr-1"></i> 28th January</span>
                    </div>  

                    <h3 className="mt-3 mb-3"><a href="blog-single.html">Marketing Strategy to bring more affect</a></h3>
                    <p className="mb-4">Non illo quas blanditiis repellendus laboriosam minima animi. Consectetur accusantium pariatur repudiandae!</p>

                    <a href="blog-single.html" className="btn btn-small btn-main btn-round-full">Learn More</a>
                  </div>
                </div>
              </div>
            </div>

            <div className="row justify-content-center mt-5">
              <div className="col-lg-6 text-center">
                <nav className="navigation pagination d-inline-block">
                    <div className="nav-links">
                        <a className="prev page-numbers" href="#">Prev</a>
                        <span aria-current="page" className="page-numbers current">1</span>
                        <a className="page-numbers" href="#">2</a>
                        <a className="next page-numbers" href="#">Next</a>
                    </div>
                </nav>
              </div>
            </div>
          </div>
        </section>

        {/* footer Start */}
        <footer className="footer section">
          <div className="container">
            <div className="row">
              <div className="col-lg-3 col-md-6 col-sm-6">
                <div className="widget">
                  <h4 className="text-capitalize mb-4">Company</h4>

                  <ul className="list-unstyled footer-menu lh-35">
                    <li><a href="#">Terms & Conditions</a></li>
                    <li><a href="#">Privacy Policy</a></li>
                    <li><a href="#">Support</a></li>
                    <li><a href="#">FAQ</a></li>
                  </ul>
                </div>
              </div>
              <div className="col-lg-2 col-md-6 col-sm-6">
                <div className="widget">
                  <h4 className="text-capitalize mb-4">Quick Links</h4>

                  <ul className="list-unstyled footer-menu lh-35">
                    <li><a href="#">About</a></li>
                    <li><a href="#">Services</a></li>
                    <li><a href="#">Team</a></li>
                    <li><a href="#">Contact</a></li>
                  </ul>
                </div>
              </div>
              <div className="col-lg-3 col-md-6 col-sm-6">
                <div className="widget">
                  <h4 className="text-capitalize mb-4">Subscribe Us</h4>
                  <p>Subscribe to get latest news article and resources  </p>

                  <form action="#" className="sub-form">
                    <input type="text" className="form-control mb-3" placeholder="Subscribe Now ..."/>
                    <a href="#" className="btn btn-main btn-small">subscribe</a>
                  </form>
                </div>
              </div>

              <div className="col-lg-3 ml-auto col-sm-6">
                <div className="widget">
                  <div className="logo mb-4">
                    <h3>Mega<span>kit.</span></h3>
                  </div>
                  <h6><a href="tel:+23-345-67890" >Support@megakit.com</a></h6>
                  <a href="mailto:support@gmail.com"><span className="text-color h4">+23-456-6588</span></a>
                </div>
              </div>
            </div>
            
            <div className="footer-btm pt-4">
              <div className="row">
                <div className="col-lg-4 col-md-12 col-sm-12">
                  <div className="copyright">
                    &copy; Copyright Reserved to <span className="text-color">Megakit.</span> by <a href="https://themefisher.com/" target="_blank">Themefisher</a>
                  </div>
                </div>

                <div className="col-lg-4 col-md-12 col-sm-12">
                  <div className="copyright">
                  Distributed by  <a href="https://themewagon.com/" target="_blank">Themewagon</a>
                  </div>
                </div>
                <div className="col-lg-4 col-md-12 col-sm-12 text-left text-lg-left">
                  <ul className="list-inline footer-socials">
                    <li className="list-inline-item"><a href="https://www.facebook.com/themefisher"><i className="ti-facebook mr-2"></i>Facebook</a></li>
                    <li className="list-inline-item"><a href="https://twitter.com/themefisher"><i className="ti-twitter mr-2"></i>Twitter</a></li>
                    <li className="list-inline-item"><a href="https://www.pinterest.com/themefisher/"><i className="ti-linkedin mr-2 "></i>Linkedin</a></li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </footer>
      </div>
    </>
  );
};

export default App;
