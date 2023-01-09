import React from 'react';

import preloaderGif from '../assets/img/preloader.gif';
import '../assets/img/css/preloader.css';

const Preloader = () => {
  return (
    <>
      <img src={preloaderGif} alt="preloader" className="preloader" />
    </>
  );
};

export default Preloader;
