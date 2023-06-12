import React, { useState, useEffect } from 'react';
import { Link, useHistory } from 'react-router-dom';
import getAxios from '../AuthAxios';
import { useAuthContext } from '../AuthContext';
import { format } from 'date-fns';




const Joke = ({ joke, onLikeClick, onDislikeClick, likes,dislikes, liked }) => {
    const { user } = useAuthContext();

    const { setup, punchline, id } = joke;

    return (
        <>
            <div className="row" style={{ minHeight: 80 }}>
                <div className="col-md-6 offset-md-3 bg-light p-4 rounded shadow">
                    <div><h4>{setup}</h4>
                        <h4>{punchline}</h4>

                        <div>
                            <div>
                                {user &&
                                    <div>
                                        <button className="btn btn-primary" onClick={onLikeClick} disabled={liked}>Like</button>
                                        <button className="btn btn-danger" onClick={onDislikeClick} disabled={liked}>Dislike</button>
                                    </div>
                                }
                            </div>
                            {!user &&
                                <div>
                                    <a href="/login">Login to your account to like/dislike this joke</a>
                                </div>
                            }
                            <br />
                            <h4>Likes: {likes}</h4>
                            <h4>Dislikes: {dislikes}</h4>
                            <h4><button className="btn btn-link" onClick={() => window.location.reload()}>Refresh</button></h4>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}


export default Joke;