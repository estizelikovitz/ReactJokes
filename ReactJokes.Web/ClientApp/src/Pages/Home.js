import { Link, useHistory } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import Joke from '../Components/Joke';
import getAxios from '../AuthAxios';
import { useAuthContext } from '../AuthContext';

const Home = (props) => {
    const [joke, setJoke] = useState({});
    const [liked, setLiked] = useState(false);
    const [likes, setLikes] = useState(0);
    const [dislikes, setDislikes] = useState(0);
    const { user } = useAuthContext();
    const history = useHistory();
    const { id } = joke;

    const getJoke = async () => {
        const { data } = await getAxios().get(`/api/jokes/getjoke`);
        setJoke(data);
    }
    const getLikes = async () => {
        const { data } = await getAxios().get(`/api/jokes/getlikesforjoke`, id);
        setLikes(data);
    }

    const getDislikes = async () => {
        const { data } = await getAxios().get(`/api/jokes/getdislikesforjoke`, id);
        setDislikes(data);
    }

    useEffect(() => {
        getJoke();
        getLikes();
        getDislikes();
        getLiked();
        console.log(likes);
    }, []);

    const onDislikeClick = async () => {
        await getAxios().post('/api/jokes/dislikejoke', { joke, user });
        setLiked(true);
        history.push('/');
    }
    const onLikeClick = async () => {
        await getAxios().post('/api/jokes/likejoke', { joke, user });
        setLiked(true);
        history.push('/');
        
    }

    const getLiked = async () => {
        const { data } = await getAxios().get('/api/jokes/wasliked', { Joke: joke, User: user });
        setLiked(data);
    }

    return (<>
        <Joke joke={joke}
            onDislikeClick={onDislikeClick}
            onLikeClick={onLikeClick}
            liked={liked}
            likes={likes}
            dislikes={dislikes}
        />
    </>)
}
    export default Home;