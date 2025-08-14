import { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Skeleton } from '@/components/ui/skeleton';

interface DataItem {
  id: string;
  title: string;
  value: string;
  status: 'active' | 'inactive' | 'pending';
  updatedAt: string;
}

// Mock API function - replace with your actual API
const fetchData = async (): Promise<DataItem[]> => {
  // Simulate API delay
  await new Promise(resolve => setTimeout(resolve, 500));
  
  // Mock data - replace with actual API call
  return Array.from({ length: 6 }, (_, i) => ({
    id: `item-${i + 1}`,
    title: `Item ${i + 1}`,
    value: `${Math.floor(Math.random() * 1000)}`,
    status: ['active', 'inactive', 'pending'][Math.floor(Math.random() * 3)] as 'active' | 'inactive' | 'pending',
    updatedAt: new Date().toLocaleTimeString(),
  }));
};

const DataGrid = () => {
  const [data, setData] = useState<DataItem[]>([]);
  const [loading, setLoading] = useState(true);

  const loadData = async () => {
    try {
      setLoading(true);
      const newData = await fetchData();
      setData(newData);
    } catch (error) {
      console.error('Error fetching data:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
    
    // Set up auto-refresh every 5 seconds
    const interval = setInterval(loadData, 5000);
    
    return () => clearInterval(interval);
  }, []);

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'active':
        return 'bg-green-500/20 text-green-400 border-green-500/50';
      case 'inactive':
        return 'bg-red-500/20 text-red-400 border-red-500/50';
      case 'pending':
        return 'bg-yellow-500/20 text-yellow-400 border-yellow-500/50';
      default:
        return 'bg-gray-500/20 text-gray-400 border-gray-500/50';
    }
  };

  if (loading && data.length === 0) {
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {Array.from({ length: 6 }).map((_, i) => (
          <Card key={i} className="border-border bg-card">
            <CardHeader>
              <Skeleton className="h-4 w-3/4" />
            </CardHeader>
            <CardContent>
              <Skeleton className="h-8 w-1/2 mb-2" />
              <Skeleton className="h-4 w-1/3" />
            </CardContent>
          </Card>
        ))}
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {data.map((item) => (
        <Card 
          key={item.id} 
          className="border-border bg-card hover:bg-gradient-secondary transition-all duration-300 hover:shadow-glow group"
        >
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium text-card-foreground">
              {item.title}
            </CardTitle>
            <Badge 
              variant="outline" 
              className={`text-xs ${getStatusColor(item.status)}`}
            >
              {item.status}
            </Badge>
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-primary group-hover:text-primary-foreground transition-colors">
              {item.value}
            </div>
            <p className="text-xs text-muted-foreground mt-2">
              Atualizado: {item.updatedAt}
            </p>
          </CardContent>
        </Card>
      ))}
    </div>
  );
};

export default DataGrid;